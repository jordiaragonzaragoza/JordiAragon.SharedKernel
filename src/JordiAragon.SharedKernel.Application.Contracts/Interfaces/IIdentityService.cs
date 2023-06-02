namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System.Threading.Tasks;
    using Ardalis.Result;

    public interface IIdentityService
    {
        Task<Result<string>> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);
    }
}