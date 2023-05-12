using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Abstractions;

namespace WebAPI.Controllers;

[ApiController]
[Route("profile/me")]
public class ProfileController : ControllerBase
{
    private IUserService _userService;
    private IHttpContextAccessor _accessor;
    
    public ProfileController(IUserService userService, IHttpContextAccessor accessor)
    {
        _userService = userService;
        _accessor = accessor;
    }
    
    [Authorize]
    [HttpGet]
    [Route("info")]
    public async Task<ActionResult<AbstractUser>> GetInfo()
    {
        var email = this._accessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "email").Value;
        var userType = this._accessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "userType").Value;

        var user = await _userService.ReadUser(email, userType);

        if (user == null)
            return NotFound("additional info not found");

        return Ok(user);
    }

    [Authorize]
    [HttpGet]
    [Route("courses")]
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
        var email = this._accessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "email").Value;
        var userType = this._accessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "userType").Value;

        return (email, userType);
    }
}