using System.Text;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Response.Models;
using WebAPI.Services.Abstractions;

namespace WebAPI.Controllers;

//[System.Web.Http.Authorize]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private IAuthService _auth;

    public AuthController(IAuthService authService)
    {
        _auth = authService;
    }

    //// POST api/auth/register
    //[HttpPost]
    //[Route("register/{type:alpha?}")]
    //public async Task<ActionResult<string>> Register([FromBody] dynamic user, string type)
    //{
    //    var isGoogle = type == "google";

    //    if (user.Equals(null))
    //        return BadRequest("Empty user provided!");

    //    return Ok(await _auth.Register(Convert.ChangeType(user, typeof(AbstractUser)), isGoogle));
    //}

    // POST api/auth/register
    [AllowAnonymous]
    [HttpPost]
    [Route("listener/register/{type:alpha?}")]
    public async Task<ActionResult<string>> Register([FromBody] Listener user, string type)
    {
        var isGoogle = type == "google";

        if (user == null)
            return BadRequest("Empty user provided!");

        return Ok(await _auth.Register(user, isGoogle));
    }

    // POST api/auth/register
    [AllowAnonymous]
    [HttpPost]
    [Route("lecturer/register/{type:alpha?}")]
    public async Task<ActionResult<string>> Register([FromBody] Lecturer user, string type)
    {
        var isGoogle = type == "google";

        if (user == null)
            return BadRequest("Empty user provided!");

        return Ok(await _auth.Register(user, isGoogle));
    }

    // POST api/auth/login
    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    //[Route("login/{type:alpha?}")]
    public async Task<ActionResult<string>> LogIn([FromBody] LoginUser loginUser)
    {
        //var isGoogle = type == "google";

        var eb = new StringBuilder();
        
        Console.WriteLine($"E: {loginUser.Email}; P: {loginUser.Password}; T: {loginUser.UserType}");
        
        if (loginUser == null)
            return BadRequest("Empty user provided!");

        //if (!isGoogle)
        //{
            if (string.IsNullOrEmpty(loginUser.Email))
                eb.Append("Email was not provided:");
            if (string.IsNullOrEmpty(loginUser.Password))
                eb.Append("Password was not provided:");
            if (string.IsNullOrEmpty(loginUser.UserType))
                eb.Append("User type was not provided:");
        //}

        if (eb.Length > 0)
            eb.Remove(eb.Length - 1, 1);

        Console.WriteLine(eb.ToString());

        if (eb.Length > 0)
            return BadRequest(error: eb.ToString());
        
        AbstractUser user = new AbstractUser();
        
        if(loginUser.UserType == "listener")
        {
            user = new Listener()
            {
                Email = loginUser.Email,
                Password = loginUser.Password
            };
        }
        else
        {
            user = new Lecturer()
            {
                Email = loginUser.Email,
                Password = loginUser.Password
            };
        }

        var operationResult = await _auth.Login(user, false);

        if (operationResult == "Log in success")
            return Ok(new Response<object>(OperationResult.OK, operationResult));
        else
            return Ok(new Response<object>(OperationResult.ERROR, operationResult));

        //return (await _auth.Login(user)) ? 
        //    Ok(new Response<object>(OperationResult.OK, "Logged in!")) : 
        //    Ok(new Response<object>(OperationResult.ERROR, "failed to log in!"));
    }

    // POST api/auth/logout
    [HttpPost]
    [Route("logout")]
    public async Task<ActionResult<string>> LogOut()
    {
         await _auth.LogOut();
         
         return Ok();
    }
    
    // [HttpGet]
    // [Route("logout")]
    // public async Task<ActionResult<string>> LogOutG()
    // {
    //     await _auth.LogOut();
    //      
    //     return Ok();
    // }
}