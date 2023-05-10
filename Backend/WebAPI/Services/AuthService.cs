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

    public AuthService(QuantEdDbContext context, HttpContext ctx, IUserService userService, IIdentityService identityService)
    {
        this._ctx = ctx;
        this._dbContext = context;
        this._userService = userService;
        this._identityService = identityService;
    }
    public async Task<bool> Login(AbstractUser user)
    {
        var dbUser = _userService.ReadUser(user);

        if (dbUser != null)
        {
            ClaimsIdentity identity = _identityService.GetIdentity(dbUser);
            
            await _ctx.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties()
            {
                IsPersistent = true
            });

            return true;

            // if (dbUser is Lecturer)
            // {
            //     identity = _identityService.GetIdentity(dbUser as Lecturer);
            // }
            // else if (dbUser is Listener)
            // {
            //     identity = _identityService.GetIdentity(dbUser as Listener);
            // }
            // else if (dbUser is Administrator)
            // {
            //     identity = _identityService.GetIdentity(dbUser as Administrator);
            // }
        }

        return false;
    }

    public async Task LogOut(AbstractUser user)
    {
        var identity = _ctx.User.Identities.SingleOrDefault();
        
        if (identity != null || identity.IsAuthenticated)
        {
           await _ctx.SignOutAsync();
        }
    }

    public async Task<string> Register(AbstractUser user, bool isGoogle)
    {
        if (_userService.UserExists(user))
        {
            if (isGoogle)
            {
                return await this.Login(user) ? "logged in" : "login failed";
            }
            else
            {
                return "Not google auth";
            }
        }
        else
        {
            return "user does not exist";
        }
        
    }

    // public Task<bool> Register(AbstractUser user)
    // {
    //     if (!this.ReadUser(user))
    //     {
    //         var result = this.AddUser(user);
    //         var claimsIdentity = new ClaimsIdentity();
    //
    //         if (user is Listener)
    //         {
    //             var listener = user as Listener;
    //
    //             var claims = new List<Claim>()
    //             {
    //                 new Claim("email", listener.Email),
    //                 new Claim("password", listener.Password)
    //             };
    //
    //             claimsIdentity = new ClaimsIdentity(claims);
    //         }
    //         else if (user is Lecturer)
    //         {
    //             var lecturer = user as Lecturer;
    //             
    //             var claims = new List<Claim>()
    //             {
    //                 new Claim("email", lecturer.Email),
    //                 new Claim("password", lecturer.Password)
    //             };
    //             
    //             claimsIdentity = new ClaimsIdentity(claims);
    //         }
    //
    //
    //         _ctx.SignInAsync(new ClaimsPrincipal(claimsIdentity));
    //     }
    //
    //     return false;
    // }
    
    private dynamic ReadUser(AbstractUser user)
    {
        if (user.GetType() == typeof(Listener))
        {
            var listener = user as Listener;

            return _dbContext.Listeners.FirstOrDefault(l => l.Email == listener.Email);
        }
        else if (user.GetType() == typeof(Lecturer))
        {
            var lecturer = user as Lecturer;

            return _dbContext.Lecturers.FirstOrDefault(l => l.Email == lecturer.Email);
        }
        else
        {
            var administrator = user as Administrator;

            return _dbContext.Administrators.FirstOrDefault(a => a.Email == administrator.Email);
        }

        return null;
    }
    
    private bool AddUser(AbstractUser user)
    {
        if (user == null)
            return false;
        
        if (user.GetType() == typeof(Listener))
        {
            var listener = user as Listener;

            try
            {
                _dbContext.Listeners.Add(listener);
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        else if (user.GetType() == typeof(Lecturer))
        {
            var lecturer = user as Lecturer;
            
            try
            {
                _dbContext.Lecturers.Add(lecturer);
            
                return true;
            }
            catch
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}