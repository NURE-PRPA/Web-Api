using Core.Models;
using DL;
using Microsoft.EntityFrameworkCore;
using WebAPI.Services.Abstractions;

namespace WebAPI.Services;

public class UserService : IUserService
{
    private QuantEdDbContext _dbContext;

    public UserService(QuantEdDbContext context)
    {
        this._dbContext = context;
    }
    
    public async Task<bool> UserExists(AbstractUser user) =>
        user == null ?
        new List<AbstractUser>()
            .Concat(_dbContext.Lecturers)
            .Concat(_dbContext.Listeners)
            .FirstOrDefault(u => u.Email == user.Email) != null 
        : false;

    public async Task<AbstractUser> ReadUser(AbstractUser user)
    {
        if (user.Equals(null))
            return null;
        
        if (user is Listener)
        {
            return _dbContext.Listeners.FirstOrDefault(u => u.Email == user.Email);
        }
        else
        {
            return _dbContext.Lecturers
                .Include(u => u.Organization)
                .FirstOrDefault(u => u.Email == user.Email);
        }
    }

    public async Task<bool> DeleteUser(AbstractUser user)
    {
        if (user.Equals(null))
            return false;
        
        try
        {
            if (user is Listener)
            {
                _dbContext.Listeners.Remove(user as Listener);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                _dbContext.Lecturers.Remove(user as Lecturer);
                await _dbContext.SaveChangesAsync();

                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AddUser(AbstractUser user)
    {
        if (user.Equals(null))
            return false;
        try
        {
            if (user is Listener)
            {
                await _dbContext.Listeners.AddAsync(user as Listener);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                await _dbContext.Lecturers.AddAsync(user as Lecturer);
                await _dbContext.SaveChangesAsync();

                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateUser(AbstractUser user)
    {
        if (user.Equals(null))
            return false;
        
        try
        {
            if (user is Listener)
            {
                _dbContext.Listeners.Update(user as Listener);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                _dbContext.Lecturers.Update(user as Lecturer);
                await _dbContext.SaveChangesAsync();

                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}