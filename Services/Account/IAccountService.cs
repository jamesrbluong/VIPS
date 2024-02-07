using Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Services.Account
{
    public interface IAccountService
    {
        Task AddToRoleAsync(AppUser user, string name, CancellationToken ct);
        Task ChangeRole(string roleName, AppUser user, CancellationToken ct);
        Task<IdentityResult> CreateAccountAsync(AppUser user, string password, CancellationToken ct);
        Task<IdentityResult> DeleteAccountAsync(AppUser user, CancellationToken ct);
        Task<string> GeneratePasswordResetTokenAsync(AppUser user, CancellationToken ct);
        Task<List<AppUser>> GetAccountsAsync(CancellationToken ct);
        Task<AppUser> GetByEmailAsync(string email, CancellationToken ct);
        Task<AppUser> GetByIdAsync(string id, CancellationToken ct);
        Task<AppUser> GetCurrentUser(string id, CancellationToken ct);
        Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken ct);
        Task<bool> HasPasswordAsync(AppUser user, CancellationToken ct);
        Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken ct);
        Task<IdentityResult> ResetPasswordAsync(AppUser user, string ResetCode, string NewPassword, CancellationToken cancellationToken);
        void SendEmail(string Email, string Code, string Purpose, string scheme, HostString host);
        Task SignOutAsync(CancellationToken ct);
        Task UpdateAsync(AppUser user, CancellationToken ct);
        Task UpdateSecurityStampAsync(AppUser user, CancellationToken ct);
        bool ValidateEmail(string email);
        bool ValidatePassword(string password);
        bool ValidatePassword(string password, string confirmPassword);
    }
}