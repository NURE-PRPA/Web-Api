using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using Core.Models;
using DL;
using Microsoft.AspNetCore.Authentication;

namespace WebAPI.Services.Abstractions;

public interface IAuthService
{
    public Task<bool> Login(AbstractUser user);
    public bool IsAuthenticated();
    public Task LogOut();
    public Task<string> Register(AbstractUser user, bool isGoogle = false);
    public (string Email, string UserType) GetCookieAuthInfo();
}