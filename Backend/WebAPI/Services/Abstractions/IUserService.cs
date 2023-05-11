using Core.Models;

namespace WebAPI.Services.Abstractions;

public interface IUserService
{
    public Task<bool> UserExists(AbstractUser user);
    public Task<AbstractUser> ReadUser(AbstractUser user); // return listener, lecturer, admin or null
    public Task<bool> DeleteUser(AbstractUser user);
    public Task<bool> AddUser(AbstractUser user);
    public Task<bool> UpdateUser(AbstractUser user);
}