using Common.Data;
using Common.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Repositories.Accounts;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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
            return await _accountRepository.GetByEmailAsync(email, ct);
        }

        public async Task<AppUser> GetByIdAsync(string id, CancellationToken ct)
        {
            return await _accountRepository.GetByIdAsync(id, ct);
        }

        public async Task<AppUser> GetCurrentUser(string id, CancellationToken ct)
        {
            return await _accountRepository.GetCurrentUser(id, ct);
        }


        public async Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken ct)
        {
            return await _accountRepository.GetRolesAsync(user, ct);
        }

        public async Task AddToRoleAsync(AppUser user, string name, CancellationToken ct)
        {
            await _accountRepository.AddToRoleAsync(user, name, ct);
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateAccountAsync(AppUser user, string password, CancellationToken ct)
        {
            return await _accountRepository.CreateAccountAsync(user, password, ct);
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> DeleteAccountAsync(AppUser user, CancellationToken ct)
        {
            return await _accountRepository.DeleteAccountAsync(user, ct);
        }

        public async Task ChangeRole(string roleName, AppUser user, CancellationToken ct)
        {
            var oldRoleNameList = await _accountRepository.GetRolesAsync(user, ct); // gets a list of all the roles the user is assigned to
            var oldRoleName = oldRoleNameList.FirstOrDefault(); // gets the first element of the list, there should only be one role per user

            if (!string.IsNullOrEmpty(oldRoleName) && !oldRoleName.Equals(roleName))
            {
                await _accountRepository.RemoveFromRoleAsync(user, oldRoleName, ct);
                await _accountRepository.AddToRoleAsync(user, roleName, ct);

                await UpdateAsync(user, ct);
            }
        }

        public async Task UpdateAsync(AppUser user, CancellationToken ct)
        {
            await _accountRepository.UpdateAsync(user, ct);
        }


        public async Task UpdateSecurityStampAsync(AppUser user, CancellationToken ct)
        {
            await _accountRepository.UpdateSecurityStampAsync(user, ct);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user, CancellationToken ct)
        {
            return await _accountRepository.GeneratePasswordResetTokenAsync(user, ct);
        }

        public async Task<bool> HasPasswordAsync(AppUser user, CancellationToken ct)
        {
            return await _accountRepository.HasPasswordAsync(user, ct);
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> ResetPasswordAsync(AppUser user, string ResetCode, string NewPassword, CancellationToken ct)
        {
            return await _accountRepository.ResetPasswordAsync(user, ResetCode, NewPassword, ct);
        }

        /**
         * signInManager Methods
         * 
         */

        public async Task SignOutAsync (CancellationToken ct)
        {
            await _accountRepository.SignOutAsync(ct);
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync (AppUser user, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken ct)
        {
            // make identityresult
            return await _accountRepository.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure, ct);
        }

        /**
         * Email Methods
         * 
         */
        public void SendEmail(string Email, string Code, string Purpose, string scheme, HostString host)
        {
            var fromEmail = new MailAddress("joshuastabile@gmail.com", "test"); // change email from mine
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "gynn cppj sxpk bxbc";

            string baseUrl = string.Format("{0}://{1}", scheme, host);

            var link = baseUrl + "/Account/" + Purpose + "?code=" + HttpUtility.UrlEncode(Code) + "&email=" + HttpUtility.UrlEncode(Email); // update url

            string subject = "";
            string body = "";

            if (Purpose == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hello," +
                    "<br>" +
                    "<br>" +
                    "We got a request to reset your account password. Please click on the link below to reset your password" +
                    "<br>" +
                    "<br>" +
                    "<a href=" + link + ">Reset Password</a>";

            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

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
