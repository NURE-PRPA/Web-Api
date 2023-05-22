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
[Route("api/containers")]
public class ContainersController : ControllerBase
{
    private QuantEdDbContext _dbContext;
    
    public ContainersController(QuantEdDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //[HttpGet]
    //[Route("{id}")]
    //public async Task<ActionResult> GetModule(string id)
    //{
    //    return await Task.Run(() =>
    //    {
    //        var module = _dbContext.Modules
    //            .Include(m => m.Test)
    //            .Include(m => m.Course)
    //            .Include(m => m.ContentContainers)
    //            .FirstOrDefault(m => m.Id == id);

    //        if (module == null)
    //            return Ok(new Response<object>(OperationResult.ERROR, "Module load error"));
    //        else
    //        {
    //            module.RemoveCycles();
    //            return Ok(new Response<CourseModule>(OperationResult.OK, module, "Module load successful"));
    //        }
    //    });
    //}

    [HttpPost]
    [Route("add")]
    public async Task<ActionResult> AddContainer([FromBody] ContentContainer container)
    {
        if (container == null)
            return Ok(new Response<object>(OperationResult.ERROR, "Empty module"));

        container.InitializeEntity();

        container.Module = _dbContext.Modules.FirstOrDefault(m => m.Id == container.Module.Id);

        await _dbContext.AddAsync(container);
        await _dbContext.SaveChangesAsync();

        container.RemoveCycles();

        return Ok(new Response<ContentContainer>(OperationResult.OK, container, "Module added successfully"));
    }
}