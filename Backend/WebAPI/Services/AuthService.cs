using System.Security.Claims;
using Core.Models;
using DL;
using Microsoft.AspNetCore.Authentication;
using WebAPI.Services.Abstractions;

namespace WebAPI.Services;


public class AuthService : IAuthService
{
    private HttpContext _ctx;
    private QuantEdDbContext _dbContext;
    private IUserService _userService;
    private IIdentityService _identityService;

    public AuthService(QuantEdDbContext context, IHttpContextAccessor accessor, IUserService userService, IIdentityService identityService)
    {
        this._ctx = accessor.HttpContext;
        this._dbContext = context;
        this._userService = userService;
        this._identityService = identityService;
    }
    public async Task<string> Login(AbstractUser user)
    {
        // if (IsAuthenticated())
        // {
        //     return true;
        // }
        
        AbstractUser dbUser = await _userService.ReadUser(user);

        if(dbUser.Password != user.Password)
        {
            return "Wrong password";
        }
        
        if (dbUser != null)
        {
            ClaimsIdentity identity = _identityService.GetIdentity(dbUser);
            
            await _ctx.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties()
            {
                IsPersistent = true
            });

            return "Log in success";
        }

        return "User does not exist";
    }

    public bool IsAuthenticated()
    {
        var identity = _ctx.User.Identities.SingleOrDefault();
        
        if (identity != null && identity.IsAuthenticated)
        {
            return true;
        }

        return false;
    }

    public async Task LogOut()
    {
        if (IsAuthenticated())
        {
            await _ctx.SignOutAsync();
        }
    }

    public async Task<string> Register(AbstractUser user, bool isGoogle)
    {
        if (await _userService.UserExists(user))
        {
            if (isGoogle)
            {
                return await Login(user);
            }
            else
            {
                return "User already registered";
            }
        }
        else
        {
           return (await _userService.AddUser(user)) ? "OK" : "Register failed";
        }
    }
    public (string Email, string UserType) GetCookieAuthInfo()
    {
        try
        {
            var email = _ctx.User.Claims
                .FirstOrDefault(c => c.Type == "email")
                .Value;

            var userType = _ctx.User.Claims
                .FirstOrDefault(c => c.Type == "userType")
                .Value;

            return (email, userType);
        }
        catch
        {
            return (null, null);
        }
    }
}