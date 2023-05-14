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
[Route("api/modules")]
public class ModulesController : ControllerBase
{
    private QuantEdDbContext _dbContext;
    private IAuthService _auth;
    private IUserService _userService;
    
    public ModulesController(QuantEdDbContext dbContext, IAuthService auth, IUserService userService)
    {
        _dbContext = dbContext;
        _auth = auth;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult> GetModule(string id)
    {
        return await Task.Run(() =>
        {
            var module = _dbContext.Modules
                .Include(m => m.Test)
                .Include(m => m.Course)
                .Include(m => m.ContentContainers)
                .FirstOrDefault(m => m.Id == id);

            if (module == null)
                return Ok(new Response<object>(OperationResult.ERROR, "Module load error"));
            else
            {
                module.RemoveCycles();
                return Ok(new Response<CourseModule>(OperationResult.OK, module, "Module load successful"));
            }
        });
    }
}