using Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Services.Account
{
    public interface IAccountService
    {
        Task ChangeRole(string roleName, AppUser user, CancellationToken ct);
        void SendEmail(string Email, string Code, string Purpose, string scheme, HostString host);
        bool ValidateEmail(string email);
        bool ValidatePassword(string password);
        bool ValidatePassword(string password, string confirmPassword);
    }
}