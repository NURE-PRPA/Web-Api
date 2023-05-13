using Core.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Response.Models;
using WebAPI.Services.Abstractions;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/profile/me")]
public class ProfileController : ControllerBase
{
    private IUserService _userService;
    private HttpContext _context;
    
    public ProfileController(IUserService userService, IHttpContextAccessor accessor)
    {
        _userService = userService;
        _context = accessor.HttpContext;
    }
    
    
    [HttpGet]
    [Route("info")]
    public async Task<ActionResult> GetInfo()
    {
        Console.WriteLine("User: " + _context.User);
        Console.WriteLine("Claims count: " + _context.User.Claims.Count());
        
        var email = _context.User.Claims
            .FirstOrDefault(c => c.Type == "email").Value;
        // var userType = this._accessor.HttpContext.User.Claims
        //     .FirstOrDefault(c => c.Type == "userType").Value;

        var userType = "listener"; // just a temporary hard coding solution
        
        Console.WriteLine("email: " + email);
        Console.WriteLine("user type: " + userType);

        var user = await _userService.ReadUser(email, userType);

        if (user == null)
            return NotFound(new Response<object>(OperationResult.OK, "user not found"));

        if (userType == "listener")
            return Ok(new Response<Listener>(OperationResult.OK, user as Listener));
        else
            return Ok(new Response<Lecturer>(OperationResult.OK, user as Lecturer));
    }
    
    [HttpGet]
    [Route("items")]
    public async Task<ActionResult> GetMyItems()
    {
        var cookieInfo = GetCookieAuthInfo();

        if (cookieInfo.UserType == "listener")
        {
            var subscriptions = _userService
                .GetListenerSubscriptions(cookieInfo.Email)
                .ToList();

            if (subscriptions == null || subscriptions.Count == 0)
                return NotFound("No subscriptions were found!");

            return Ok(subscriptions);
        }
        else
        {
            var courses = _userService
                .GetLecturerCourses(cookieInfo.Email)
                .ToList();

            if (courses == null || courses.Count == 0)
                return NotFound("No courses were found!");

            return Ok(courses);
        }
    }

    private (string Email, string UserType) GetCookieAuthInfo()
    {
        var email = _context.User.Claims
            .FirstOrDefault(c => c.Type == "email").Value;
        var userType = _context.User.Claims
            .FirstOrDefault(c => c.Type == "userType").Value;

        return (email, userType);
    }
}