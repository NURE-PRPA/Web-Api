using System.ComponentModel;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Abstractions;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private IAuthService _auth;
    
    public AuthController(IAuthService authService)
    {
        _auth = authService;
    }

    
    // POST api/auth/register
    [HttpPost]
    [Route("register/{type:alpha?}")]
    public async Task<ActionResult<string>> Register([FromBody] AbstractUser user, string type)
    {
        var isGoogle = type == "google";
        
        if (user.Equals(null))
            return BadRequest("Empty user provided!");

        return Ok(await _auth.Register(user, isGoogle));
    }

    // POST api/auth/login
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<string>> LogIn([FromBody] AbstractUser user)
    {
        if (user.Equals(null))
            return BadRequest("Empty user provided!");

        return (await _auth.Login(user)) ? Ok("Logged in") : Ok("Login failed");
    }
    
    // POST api/auth/logout
    [HttpPost]
    [Route("logout")]
    public async Task<ActionResult<string>> LogOut()
    {
         await _auth.LogOut();
         
         return Ok();
    }
}

// {
// "id": 0,
// "firstName": "Michael",
// "lastName": "Tkachenko",
// "gender": false,
// "dob": {
//     "year": 2003,
//     "month": 1,
//     "day": 24,
//     "dayOfWeek": 0
// },
// "email": "20werasdf@gmail.com",
// "date": "2023-05-11T12:41:15.557Z",
// "googleId": 0,
// "password": "123456",
// "banCount": 0,
// "bans": null,
// "subscriptions": null
// }