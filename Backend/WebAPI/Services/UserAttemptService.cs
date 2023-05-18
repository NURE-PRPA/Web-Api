using Core.Models;
using DL;
using Microsoft.EntityFrameworkCore;
using WebAPI.Services.Abstractions;

namespace WebAPI.Services;

public class UserAttemptService : IUserAttemptService
{
    private IUserService _userService;
    private QuantEdDbContext _dbContext;
    private IAuthService _authService;
    public UserAttemptService(IUserService userService, QuantEdDbContext dbContext, IAuthService authService)
    {
        _userService = userService;
        _dbContext = dbContext;
        _authService = authService;
    }
    public async Task<UserAttempt> GetAttempt(string testId)
    {
        var user = _authService.GetCookieAuthInfo();
        if (user.Email == null)
            return null;

        var test = _dbContext.Tests
            .Include(t => t.Module)
            .ThenInclude(m => m.Course)
            .FirstOrDefault(x => x.Id == testId);

        if (test == null)
            return null;

        var subscription = _dbContext.Subscriptions
            .Include(s => s.Listener)
            .Include(s => s.Course)
            .FirstOrDefault(s => s.Course.Id == test.Module.Course.Id && s.Listener.Email == user.Email);

        if (subscription == null)
            return null;

        // Temporary implementations
        var userAttempts = new List<UserAttempt>();

        var userAttempt = userAttempts.FirstOrDefault(a => a.TestId == testId && a.SubscriptionId == subscription.Id);

        return userAttempt;
    }
    public async Task AddAttempt(string testId, byte mark)
    {

    }
}