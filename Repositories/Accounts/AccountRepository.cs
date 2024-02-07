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

        public Task<List<Common.Entities.AppUser>> GetListAsync (CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return _userManager.Users.ToListAsync(cancellationToken);
            }

            return default;
        }

        public async Task<AppUser> GetByEmailAsync (string email, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _userManager.FindByEmailAsync(email);
            }

            return default;
        }

        public async Task<AppUser> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _userManager.FindByIdAsync(id);
            }

            return default;
            
        }

        public async Task<AppUser> GetCurrentUser(string id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _userManager.FindByIdAsync(id);
            }

            return default;
        }


        public async Task SignOutAsync (CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _signInManager.SignOutAsync();
                return;
            }
            
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _signInManager.PasswordSignInAsync(user, password, false, true);
            }

            return default;
        }

        public async Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _userManager.GetRolesAsync(user);
            }

            return default;
        }

        public async Task AddToRoleAsync(AppUser user, string name, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _userManager.AddToRoleAsync(user, name);
                return;
            }
        }

        public async Task RemoveFromRoleAsync(AppUser user, string name, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _userManager.RemoveFromRoleAsync(user, name);
                return;
            }
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateAccountAsync(AppUser user, string password, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _userManager.CreateAsync(user, password);
            }

            return default;
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> DeleteAccountAsync(AppUser user, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _userManager.DeleteAsync(user);
            }

            return default;
        }

        public async Task UpdateAsync (AppUser user, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _userManager.UpdateAsync(user);
                return;
            }
        }

        public async Task UpdateSecurityStampAsync(AppUser user, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return;
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _userManager.GeneratePasswordResetTokenAsync(user);
            }

            return default;
        }

        public async Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _userManager.HasPasswordAsync(user);
            }

            return false;
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> ResetPasswordAsync(AppUser user, string ResetCode, string NewPassword, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _userManager.ResetPasswordAsync(user, ResetCode, NewPassword);
            }

            return default;
        }
    }
}