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
[Route("api/organizations")]
public class OrganizationsController : ControllerBase
{
    private QuantEdDbContext _dbContext;
    
    public OrganizationsController(QuantEdDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [AllowAnonymous]
    [HttpGet]
    [Route("all")]
    public async Task<ActionResult> GetAllOrganizations()
    {
        return await Task.Run(() =>
        {
            var organizations = _dbContext.Organizations.ToList();

            return Ok(new Response<List<Organization>>(OperationResult.OK, organizations, "Courses load successful"));
        });
    }
}