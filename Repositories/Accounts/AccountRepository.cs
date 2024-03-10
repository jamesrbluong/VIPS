using Common.Data;
using Common.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Repositories.Accounts
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountRepository(Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<List<Common.Entities.AppUser>> GetListAsync(CancellationToken cancellationToken)
        {
            return await _userManager.Users.ToListAsync(cancellationToken);
        }

        public Task<AppUser> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<AppUser> GetCurrentUser(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userManager.FindByIdAsync(id);
        }


        public async Task SignOutAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _signInManager.SignOutAsync();
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _signInManager.PasswordSignInAsync(user, password, false, true);
        }

        public async Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userManager.GetRolesAsync(user);
        }

        public async Task AddToRoleAsync(AppUser user, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _userManager.AddToRoleAsync(user, name);
        }

        public async Task RemoveFromRoleAsync(AppUser user, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _userManager.RemoveFromRoleAsync(user, name);
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateAccountAsync(AppUser user, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> DeleteAccountAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userManager.DeleteAsync(user);
        }

        public async Task UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateSecurityStampAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userManager.HasPasswordAsync(user);
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> ResetPasswordAsync(AppUser user, string ResetCode, string NewPassword, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userManager.ResetPasswordAsync(user, ResetCode, NewPassword);
        }
    }
}