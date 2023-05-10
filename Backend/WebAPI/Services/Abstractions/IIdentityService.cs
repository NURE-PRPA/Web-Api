using System.Security.Claims;
using Core.Models;

namespace WebAPI.Services.Abstractions;

public interface IIdentityService
{
    public ClaimsIdentity GetIdentity(AbstractUser user);
}