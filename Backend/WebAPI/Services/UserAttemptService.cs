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

        var userAttempt = _dbContext.UserAttempts.FirstOrDefault(a => a.TestId == testId && a.SubscriptionId == subscription.Id);

        return userAttempt;
    }

    public async Task<List<UserAttempt>> GetAttempts(string courseId)
    {
        var user = _authService.GetCookieAuthInfo();
        if (user.Email == null)
            return null;

        var tests = _dbContext.Tests
            .Include(t => t.Module)
            .ThenInclude(m => m.Course)
            .Where(x => x.Module.Course.Id == courseId)
            .Select(t => t)
            .ToList();

        if (tests == null)
            return null;

        var subscription = _dbContext.Subscriptions
            .Include(s => s.Listener)
            .Include(s => s.Course)
            .FirstOrDefault(s => s.Course.Id == courseId && s.Listener.Email == user.Email);

        if (subscription == null)
            return null;

        var userAttempts = new List<UserAttempt>();

        foreach ( var test in tests)
        {
            var userAttempt = _dbContext.UserAttempts.FirstOrDefault(a => a.TestId == test.Id && a.SubscriptionId == subscription.Id);
            if (userAttempt != null)
                userAttempts.Add(userAttempt);
        }

        return userAttempts;
    }
    public async Task<bool> AddAttempt(TestResult result)
    {
        var user = _authService.GetCookieAuthInfo();
        if (user.Email == null)
            return false;

        var test = _dbContext.Tests
            .Include(t => t.Module)
            .ThenInclude(m => m.Course)
            .FirstOrDefault(x => x.Id == result.TestId);

        if (test == null)
            return false;

        var subscription = _dbContext.Subscriptions
            .Include(s => s.Listener)
            .Include(s => s.Course)
            .FirstOrDefault(s => s.Course.Id == test.Module.Course.Id && s.Listener.Email == user.Email);

        if (subscription == null)
            return false;

        var userAttempt = _dbContext.UserAttempts.FirstOrDefault(a => a.TestId == result.TestId && a.SubscriptionId == subscription.Id);

        if(userAttempt != null)
        {
            userAttempt.Mark = result.Mark;
            userAttempt.TimeStamp = DateTime.Now;

            _dbContext.UserAttempts.Update(userAttempt);
            _dbContext.SaveChanges();

            AddCertificate(subscription);

            return true;
        }

        userAttempt = new UserAttempt()
        {
            Mark = result.Mark,
            TestId = test.Id,
            SubscriptionId = subscription.Id
        };
        userAttempt.InitializeEntity();

        await _dbContext.UserAttempts.AddAsync(userAttempt);
        _dbContext.SaveChanges();

        AddCertificate(subscription);

        return true;
    }

    private async Task AddCertificate(Subscription subscription)
    {
        var attemptCount = 0;
        int mark = 0;

        var tests = _dbContext.Modules
            .Include(m => m.Course)
            .Include(m => m.Test)
            .ThenInclude(t => t.Questions)
            .Where(m => m.Test != null && m.Course.Id == subscription.Course.Id)
            .Select(m => m.Test)
            .ToList();

        foreach ( var test in tests)
        {
            var userAttempt = await GetAttempt(test.Id);
            
            if (userAttempt != null)
            {
                attemptCount++;
                if (test.Questions.Count != 0)
                    mark += (byte)Math.Ceiling((double)userAttempt.Mark / test.Questions.Count * 100);
            }
        }

        if(attemptCount == tests.Count())
        {
            var certificate = _dbContext.Certificates
                .Include(c => c.Subscription)
                .FirstOrDefault(c => c.Subscription.Id == subscription.Id);

            if(certificate == null)
            {
                byte totalMark = (byte)Math.Ceiling((double)mark / attemptCount);
                certificate = new Certificate()
                {
                    Mark = totalMark,
                    Subscription = subscription
                };
                certificate.InitializeEntity();

                await _dbContext.Certificates.AddAsync(certificate);
                _dbContext.SaveChanges();
            }
        }
    }
}