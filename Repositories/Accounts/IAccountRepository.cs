using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Entities;
using Microsoft.AspNetCore.Identity;

namespace Repositories.Accounts
{
    public interface IAccountRepository
    {
        Task<List<AppUser>> GetListAsync(CancellationToken cancellationToken);
        Task<AppUser> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task SignOutAsync(CancellationToken cancellationToken);
        Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken ct);
        Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken);
        Task<IdentityResult> CreateAccountAsync(AppUser user, string password, CancellationToken cancellationToken);
        Task AddToRoleAsync(AppUser user, string name, CancellationToken cancellationToken);
        Task<AppUser> GetCurrentUser(string id, CancellationToken ct);
        Task<AppUser> GetByIdAsync(string id, CancellationToken ct);
        Task<IdentityResult> DeleteAccountAsync(AppUser user, CancellationToken cancellationToken);
        Task RemoveFromRoleAsync(AppUser user, string name, CancellationToken cancellationToken);
        Task UpdateAsync(AppUser user, CancellationToken cancellationToken);
        Task UpdateSecurityStampAsync(AppUser user, CancellationToken cancellationToken);
        Task<string> GeneratePasswordResetTokenAsync(AppUser user, CancellationToken cancellationToken);
        Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken);
        Task<IdentityResult> ResetPasswordAsync(AppUser user, string ResetCode, string NewPassword, CancellationToken cancellationToken);
    }
}