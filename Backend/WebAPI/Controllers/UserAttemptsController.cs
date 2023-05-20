using DL;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Response.Models;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Mysqlx;
using System.Runtime.CompilerServices;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/attempts")]
public class UserAttemptsController : ControllerBase
{
    private IUserAttemptService _userAttemptService;
    
    public UserAttemptsController(IUserAttemptService userAttemptService)
    {
        _userAttemptService = userAttemptService;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetAttempt(string testId)
    {
        var userAttempt = await _userAttemptService.GetAttempt(testId);

        if (userAttempt == null)
            return Ok(new Response<object>(OperationResult.ERROR, "Attempt load error"));
        else
            return Ok(new Response<UserAttempt>(OperationResult.OK, userAttempt, "Attempt load successful"));
    }

    [HttpPost]
    [Route("add")]
    public async Task<ActionResult> AddAttempt(TestResult result)
    {
        if (result == null)
            return Ok(new Response<object>(OperationResult.ERROR, "Empty input"));

        _userAttemptService.AddAttempt(result);

        return Ok(new Response<object>(OperationResult.OK, "Attempt added successfully"));
    }
}