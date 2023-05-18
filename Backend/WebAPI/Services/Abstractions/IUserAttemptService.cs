using Core.Models;

namespace WebAPI.Services.Abstractions;

public interface IUserAttemptService
{
    public Task<UserAttempt> GetAttempt(string testId);
    public Task AddAttempt(string testId, byte mark);
}