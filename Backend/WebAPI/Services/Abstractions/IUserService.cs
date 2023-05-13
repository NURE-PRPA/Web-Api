using Core.Models;

namespace WebAPI.Services.Abstractions;

public interface IUserService
{
    public Task<bool> UserExists(AbstractUser user);
    public Task<AbstractUser> ReadUser(AbstractUser user); // return listener, lecturer, admin or null
    public Task<AbstractUser> ReadUser(string email, string userType);
    public Task<bool> DeleteUser(AbstractUser user);
    public Task<bool> AddUser(AbstractUser user);
    public Task<bool> UpdateUser(AbstractUser user);
    public List<Subscription> GetListenerSubscriptions(string email);
    public IQueryable<List<Course>> GetLecturerCourses(string email);
}