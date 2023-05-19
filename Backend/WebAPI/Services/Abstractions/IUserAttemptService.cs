using Core.Models;

namespace WebAPI.Services.Abstractions;

public interface IUserAttemptService
{
    public Task<UserAttempt> GetAttempt(string testId);
    public Task<List<UserAttempt>> GetAttempts(string courseId);
    public Task<bool> AddAttempt(TestResult result);
}