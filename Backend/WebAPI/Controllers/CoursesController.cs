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
                .FirstOrDefault(c => c.Id == id);

            course.Lecturer.Courses = null;

            return Ok(new Response<Course>(OperationResult.OK, course, "Course load successful"));
        });
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("modules/{id:int}")]
    public async Task<ActionResult> GetModule(string id)
    {
        return await Task.Run(() =>
        {
            var module = _dbContext.Modules
                .Include(m => m.Test)
                .Include(m => m.Course)
                .Include(m => m.ContentContainers)
                .FirstOrDefault(m => m.Id == id);

            if(module == null)
                return Ok(new Response<object>(OperationResult.ERROR, "Module load error"));
            else
            {
                module.RemoveCycles();
                return Ok(new Response<CourseModule>(OperationResult.OK, module, "Module load successful"));
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
}