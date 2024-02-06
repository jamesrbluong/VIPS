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

        public async Task SignOutAsync (CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _signInManager.SignOutAsync();
            }
            
        }

        public async Task PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _signInManager.PasswordSignInAsync(user, password, false, true);
            }
        }

        public async Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _userManager.GetRolesAsync(user);
            }

            return default;
        }

    }
}