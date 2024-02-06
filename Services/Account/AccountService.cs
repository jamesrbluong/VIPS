using Common.Data;
using Common.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Repositories.Accounts;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        // private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;
        // private readonly SignInManager<AppUser> _signInManager;

        public AccountService(ApplicationDbContext db, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            // _userManager = userManager;
            // _signInManager = signInManager;
            
        }

        /**
         * Repository Methods
         * 
         */
        public async Task<List<Common.Entities.AppUser>> GetAccountsAsync(CancellationToken ct)
        {
            return await _accountRepository.GetListAsync(ct);
        }

        public async Task<AppUser> GetByEmailAsync (string email, CancellationToken ct)
        {
            if (ValidateEmail(email) == false || ValidatePassword(email) == false)
            {
                return default; // View("Login", model);
            }

            return await _accountRepository.GetByEmailAsync(email, ct);
        }


        /**
         * signInManager Methods
         * 
         */

        public async Task SignOutAsync (CancellationToken ct)
        {
            await _accountRepository.SignOutAsync(ct);
        }

        public async Task PasswordSignInAsync (AppUser user, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken ct)
        {
            await _accountRepository.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure, ct);
        }

        public async Task<IList<string>> GetRolesAsync (AppUser user, CancellationToken ct)
        {
            if (user != null)
            {
                return await _accountRepository.GetRolesAsync(user, ct);
            }

            return default;
        }


        /**
         * Input Validation Methods
         * 
         */

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                // TempData["error"] = "Email must not be empty";
                return false;
            }

            if (!email.Contains("@unf.edu"))
            {
                // TempData["error"] = "Email must contain \"@unf.edu\"";
                return false;
            }

            return true;
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                // TempData["error"] = "Password must not be empty";
                return false;
            }

            return true;
        }
        public bool ValidatePassword(string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                // TempData["error"] = "Password must not be empty";
                return false;
            }
            if (!password.Equals(confirmPassword))
            {
                // TempData["error"] = "Passwords do not match";
                return false;
            }

            return true;
        }
    }
}
