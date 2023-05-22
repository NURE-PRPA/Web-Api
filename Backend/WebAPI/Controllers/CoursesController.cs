using DL;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Response.Models;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Mysqlx;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private QuantEdDbContext _dbContext;
    private IAuthService _auth;
    private IUserService _userService;
    private IUserAttemptService _userAttemptService;
    
    public CoursesController(QuantEdDbContext dbContext, IAuthService auth, IUserService userService, IUserAttemptService userAttemptService)
    {
        _dbContext = dbContext;
        _auth = auth;
        _userService = userService;
        _userAttemptService = userAttemptService;
    }
    
    [AllowAnonymous]
    [HttpGet]
    [Route("all")]
    public async Task<ActionResult> GetAllCourses()
    {
        return await Task.Run(async () =>
        {
            var courses = _dbContext.Courses
                .Include(c => c.Modules)
                .Include(c => c.Lecturer)
                .ThenInclude(l => l.Organization)
                .ToList();

            if(courses != null)
            {
                foreach (var course in courses)
                {
                    course.IsAcquired = await IsCourseAcquired(course.Id);
                }
                foreach (var course in courses)
                {
                    course.RemoveCycles();
                }
            }

            return Ok(new Response<List<Course>>(OperationResult.OK, courses, "Courses load successful"));
        });
    }

    [HttpGet]
    [Route("my")]
    public async Task<ActionResult> GetMyCourses()
    {
        var user = _auth.GetCookieAuthInfo();

        if (user.Email == null)
            return Unauthorized(new { error = "Not authenticated" });

        var courses = new List<Course>();

        if(user.UserType == "listener")
        {
            courses = _dbContext.Subscriptions
            .Include(s => s.Listener)
            .Include(s => s.Course)
            .ThenInclude(c => c.Modules)
            .Include(s => s.Course)
            .ThenInclude(c => c.Lecturer)
            .ThenInclude(l => l.Organization)
            .Where(s => s.Listener.Email == user.Email)
            .Select(s => s.Course)
            .ToList();

            if (courses != null)
            {
                foreach (var course in courses)
                {
                    course.IsAcquired = await IsCourseAcquired(course.Id);
                }
            }
        }
        else if(user.UserType == "lecturer")
        {
            courses = _dbContext.Courses
                .Include(c => c.Lecturer)
                .ThenInclude(l => l.Organization)
                .Include(c => c.Modules)
                .Where(c => c.Lecturer.Email == user.Email)
                .Select(c => c)
                .ToList();
        }

        if (courses != null)
        {
            foreach (var course in courses)
            {
                course.RemoveCycles();
            }
        }

        return Ok(new Response<List<Course>>(OperationResult.OK, courses, "Courses load successful"));
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetCourse(string id)
    {
        return await Task.Run(async () =>
        {
            var course = _dbContext.Courses
                .Include(c => c.Modules)
                .ThenInclude(m => m.Test)
                .ThenInclude(t => t.Questions)
                .Include(c => c.Modules)
                .ThenInclude(m => m.ContentContainers)
                .Include(c => c.Lecturer)
                .ThenInclude(l => l.Organization)
                .FirstOrDefault(c => c.Id == id);

            if (course == null)
                return Ok(new Response<object>(OperationResult.ERROR, "Course load error"));
            else
            {
                var attemptCount = 0;
                var testCount = 0;

                foreach (var module in course.Modules)
                {
                    if(module.Test != null)
                    {
                        var userAttempt = await _userAttemptService.GetAttempt(module.Test.Id);
                        if(userAttempt != null)
                        {
                            module.Test.UserAttempts = new List<UserAttempt>
                            {
                                userAttempt
                            };
                            attemptCount++;
                        }
                        testCount++;
                    }
                }

                if (testCount == 0)
                    course.Progress = 0;
                else
                    course.Progress = (byte)Math.Ceiling((double)((attemptCount / testCount) * 100));

                course.IsAcquired = await IsCourseAcquired(course.Id);

                course.RemoveCycles();
                return Ok(new Response<Course>(OperationResult.OK, course, "Course load successful"));
            }
        });
    }

    //[HttpGet]
    //[Route("authorize/{courseId}")]
    //public async Task<ActionResult> IsCourseAcquired(string courseId)
    //{
    //    var user = _auth.GetCookieAuthInfo();

    //    if (user.Email == null)
    //        return Unauthorized(new { error = "Not authenticated" });
    //        // return Ok(new Response<bool>(OperationResult.ERROR, false, "not authenticated"));
        
    //    var isAuthorized = _userService
    //        .GetListenerSubscriptions(user.Email)
    //        .FirstOrDefault(s => s.Course.Id == courseId) != null;
        
    //    return Ok(new Response<bool>(OperationResult.OK, isAuthorized));
    //}

    [HttpPost]
    [Route("add")]
    public async Task<ActionResult> AddCourse(Course course)
    {
        var user = _auth.GetCookieAuthInfo();

        if (user.Email == null)
            return Unauthorized(new { error = "Not authenticated" });

        if (course == null)
            return Ok(new Response<object>(OperationResult.ERROR, "Empty course"));

        course.InitializeEntity();

        course.Lecturer = _dbContext.Lecturers.FirstOrDefault(l => l.Email == user.Email);

        await _dbContext.AddAsync(course);
        await _dbContext.SaveChangesAsync();

        course.RemoveCycles();

        return Ok(new Response<Course>(OperationResult.OK, course, "Course added successfully"));
    }

    [HttpPost]
    [Route("enroll/{courseId}")]
    public async Task<ActionResult> Enroll(string courseId)
    {
        var user = _auth.GetCookieAuthInfo();

        if (user.Email == null)
            return Unauthorized(new { error = "Not authenticated" });
        // return Ok(new Response<bool>(OperationResult.ERROR, false, "not authenticated"));

        var checkSubsctiption = _dbContext.Subscriptions
            .Include(s => s.Course)
            .Include(s => s.Listener)
            .FirstOrDefault(s => s.Course.Id == courseId && s.Listener.Email == user.Email) == null;

        if (checkSubsctiption)
        {
            var subscription = new Subscription()
            {
                IsActive = true,
                Listener = _dbContext.Listeners.FirstOrDefault(l => l.Email == user.Email),
                Course = _dbContext.Courses.FirstOrDefault(c => c.Id == courseId)
            };
            subscription.InitializeEntity();

            await _dbContext.AddAsync(subscription);
            _dbContext.SaveChanges();

            return Ok(new Response<bool>(OperationResult.OK));
        }

        return Ok(new Response<bool>(OperationResult.ERROR));
    }

    public async Task<bool> IsCourseAcquired(string courseId)
    {
        var user = _auth.GetCookieAuthInfo();

        if (user.Email == null)
            return false;
        // return Ok(new Response<bool>(OperationResult.ERROR, false, "not authenticated"));

        var subscriptions = _userService
            .GetListenerSubscriptions(user.Email);

        var isAuthorized = false;

        if (subscriptions != null && subscriptions.Count != 0)
            isAuthorized = subscriptions.FirstOrDefault(s => s.Course.Id == courseId) != null;

        return isAuthorized;
    }
}