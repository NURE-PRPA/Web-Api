using System.Net;
using System.Security.Claims;
using Core.Models;
using DL;
using Microsoft.AspNetCore.Authentication;

namespace WebAPI.Services;

public interface IAuthService
{
    public void Login(AbstractUser user);
    public void LogOut(AbstractUser user);
}

public class RegisterServiceFabric
{
    private HttpContext _ctx;
    
    public RegisterServiceFabric(HttpContext ctx) => this._ctx = ctx;
    
    public IRegisterService GetRegisterService(AbstractUser user)
    {
        if (user.GetType() == typeof(Listener))
        {
            return new ListenerRegisterService(_ctx);
        }
        else if (user.GetType() == typeof(Lecturer))
        {
            return new LecturerRegisterService(_ctx);
        }
        else
        {
            return new AdministratorRegisterService(_ctx);
        }
    }
}

public abstract class IRegisterService
{
    protected HttpContext _ctx;
    private QuantEdDbContext _context;
    
    protected IRegisterService(HttpContext ctx) => this._ctx = ctx;
    
    public void Register(AbstractUser user, AuthenticationScheme scheme)
    {
        return;
    }

    protected bool UserExists(AbstractUser user)
    {
        if (user.GetType() == typeof(Listener))
        {
            var listener = user as Listener;

            return _context.Listeners.FirstOrDefault(l => l.Email == listener.Email) != null;
        }
        else if (user.GetType() == typeof(Lecturer))
        {
            var lecturer = user as Lecturer;

            //return _context.Lecturers.FirstOrDefault(l => l.Email == lecturer.Email) != null;
        }

        return false;
    }

    protected bool AddUser(AbstractUser user)
    {
        if (user == null)
            return false;
        
        if (user.GetType() == typeof(Listener))
        {
            var listener = user as Listener;

            try
            {
                _context.Listeners.Add(listener);
                
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
                _context.Lecturers.Add(lecturer);
            
                return true;
            }
            catch
            {
                return false;
            }
            return false;
        }
        else
        {
            var administrator = user as Administrator;

            try
            {
                _context.Administrators.Add(administrator);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}

public class ListenerRegisterService : IRegisterService
{
    public ListenerRegisterService(HttpContext ctx) : base(ctx) { }
    
    public void Register(AbstractUser user, AuthenticationScheme scheme)
    {
        var listener = user as Listener;
        
        if (scheme.Name == "Google")
        {
            // to do
        }
        else
        {
            var properties = new AuthenticationProperties
            {
                IsPersistent = true // Set the cookie to be persistent if the user wants to be remembered
            };
            _ctx.SignInAsync(new ClaimsPrincipal(), properties); // claims property not empty!!!
            return;
        }
    }
}

public class LecturerRegisterService : IRegisterService
{
    public LecturerRegisterService(HttpContext ctx) : base(ctx) { }
    public void Register(AbstractUser user)
    {
        var lecturer = user as Lecturer;
        // register lecturer
        
        return;
    }
}

public class AdministratorRegisterService : IRegisterService
{
    public AdministratorRegisterService(HttpContext ctx) : base(ctx) { }
    
    public void Register(AbstractUser user)
    {
        var administrator = user as Administrator;
        //register administrator

        return;
    }
}