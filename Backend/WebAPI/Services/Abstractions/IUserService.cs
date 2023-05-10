using Core.Models;

namespace WebAPI.Services.Abstractions;

public interface IUserService
{
    public bool UserExists(AbstractUser user);
    public dynamic ReadUser(AbstractUser user); // return listener, lecturer, admin or null
    public bool DeleteUser(AbstractUser user);
    public bool AddUser(AbstractUser user);
    public dynamic UpdateUser(AbstractUser user);
}