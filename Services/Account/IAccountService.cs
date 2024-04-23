using Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Services.Account
{
    public interface IAccountService
    {
        Task ChangeRole(string roleName, AppUser user, CancellationToken ct);
        Task<List<AppUser>> SearchAccounts(string query, CancellationToken ct);
        void SendEmail(string Email, string Code, string Purpose, string scheme, HostString host);
        string ValidateEmail(string email);
        string ValidatePassword(string password);
        string ValidatePassword(string password, string confirmPassword);
    }
}