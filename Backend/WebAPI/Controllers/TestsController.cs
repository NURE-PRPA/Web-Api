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
[Route("api/tests")]
public class TestsController : ControllerBase
{
    private QuantEdDbContext _dbContext;
    private IAuthService _auth;
    private IUserService _userService;
    
    public TestsController(QuantEdDbContext dbContext, IAuthService auth, IUserService userService)
    {
        _dbContext = dbContext;
        _auth = auth;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult> GetTest(string id)
    {
        return await Task.Run(() =>
        {
            var test = _dbContext.Tests
                .Include(t => t.Module)
                .Include(t => t.Questions)
                .FirstOrDefault(t => t.Id == id);

            if (test == null)
                return Ok(new Response<object>(OperationResult.ERROR, "Test load error"));
            else
            {
                test.RemoveCycles();
                return Ok(new Response<Test>(OperationResult.OK, test, "Test load successful"));
            }
        });
    }
}