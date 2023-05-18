using System.Security.Claims;
using Core.Models;
using WebAPI.Services.Abstractions;

namespace WebAPI.Services;

public class IdentityService : IIdentityService
{
    public ClaimsIdentity GetIdentity(AbstractUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim("email", user.Email),
            new Claim("password", user.Password)
        };

        if (user is Listener)
            user.UserType = "listener";
        else
            user.UserType = "lecturer";

        claims.Add(new Claim("userType", user.UserType));

        return new ClaimsIdentity(claims, "cookie");
    }
}