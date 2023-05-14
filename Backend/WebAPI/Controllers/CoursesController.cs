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
    
    public CoursesController(QuantEdDbContext dbContext, IAuthService auth, IUserService userService)
    {
        _dbContext = dbContext;
        _auth = auth;
        _userService = userService;
    }
    
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("all")]
    public async Task<ActionResult> GetAllCourses()
    {
        return await Task.Run(() =>
        {
            var courses = _dbContext.Courses.ToList();

            return Ok(new Response<List<Course>>(OperationResult.OK, courses, "Courses load successful"));
        });
    }
    
    [AllowAnonymous]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult> GetCourse(string id)
    {
        return await Task.Run(() =>
        {
            var course = _dbContext.Courses
                .Include(c => c.Lecturer)
                .Include(c => c.Modules)
                .FirstOrDefault(c => c.Id == id);

            if (course == null)
                return Ok(new Response<object>(OperationResult.ERROR, "Course load error"));
            else
            {
                course.RemoveCycles();
                return Ok(new Response<Course>(OperationResult.OK, course, "Course load successful"));
            }
        });
    }

    [HttpGet]
    [Route("authorize/{courseId:int}")]
    public async Task<ActionResult> IsCourseAcquired(string courseId)
    {
        var user = _auth.GetCookieAuthInfo();

        if (user.Email == null)
            return Unauthorized(new { error = "Not authenticated" });
            // return Ok(new Response<bool>(OperationResult.ERROR, false, "not authenticated"));
        
        var isAuthorized = _userService
            .GetListenerSubscriptions(user.Email)
            .FirstOrDefault(s => s.Course.Id == courseId) != null;
        
        return Ok(new Response<bool>(OperationResult.OK, isAuthorized));
    }

    [HttpPost]
    [Route("add")]
    public async Task<ActionResult> AddCourse(Course course)
    {
        if (course == null)
            return Ok(new Response<object>(OperationResult.ERROR, "Empty course"));

        course.SetInitialData();

        course.Lecturer = _dbContext.Lecturers.FirstOrDefault(l => l.Id == course.Lecturer.Id);

        await _dbContext.AddAsync(course);
        await _dbContext.SaveChangesAsync();

        course.RemoveCycles();

        return Ok(new Response<Course>(OperationResult.OK, course, "Course added successfully"));
    }
}