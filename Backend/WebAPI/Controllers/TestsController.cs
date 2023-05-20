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
    
    public TestsController(QuantEdDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetTest(string id)
    {
        return await Task.Run(() =>
        {
            var test = _dbContext.Tests
                .Include(t => t.Module)
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
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

    //[HttpPost]
    //[Route("add")]
    //public async Task<ActionResult> AddTest(Test test)
    //{
    //    if (test == null)
    //        return Ok(new Response<object>(OperationResult.ERROR, "Empty test"));

    //    test.InitializeEntity();

    //    test.Module = _dbContext.Modules.FirstOrDefault(m => m.Id == test.Module.Id);

    //    await _dbContext.AddAsync(test);
    //    await _dbContext.SaveChangesAsync();

    //    test.RemoveCycles();

    //    return Ok(new Response<Test>(OperationResult.OK, test, "Test added successfully"));
    //}
}