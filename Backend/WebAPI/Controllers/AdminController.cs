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
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private QuantEdDbContext _dbContext;
    private IAuthService _auth;
    private IUserService _userService;
    
    public AdminController(QuantEdDbContext dbContext, IAuthService auth, IUserService userService)
    {
        _dbContext = dbContext;
        _auth = auth;
        _userService = userService;
    }
}