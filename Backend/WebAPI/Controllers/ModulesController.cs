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
    
    public ModulesController(QuantEdDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("{id}")]
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

    [HttpPost]
    [Route("add")]
    public async Task<ActionResult> AddModule(CourseModule module)
    {
        if (module == null)
            return Ok(new Response<object>(OperationResult.ERROR, "Empty module"));

        module.InitializeEntity();

        module.Course = _dbContext.Courses.FirstOrDefault(c => c.Id == module.CourseId);

        await _dbContext.AddAsync(module);
        await _dbContext.SaveChangesAsync();

        module.RemoveCycles();

        return Ok(new Response<CourseModule>(OperationResult.OK, module, "Module added successfully"));
    }
}