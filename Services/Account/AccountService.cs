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
        
        public async Task<List<AppUser>> SearchAccounts (string query, CancellationToken ct)
        {
            var result = (await _accountRepository.GetListAsync(ct))
                .Where(a => a.Email.ToLower().Contains(query.ToLower())).ToList();

            return result;
        }

        public async Task ChangeRole(string roleName, AppUser user, CancellationToken ct)
        {
            var oldRoleNameList = await _accountRepository.GetRolesAsync(user, ct); // gets a list of all the roles the user is assigned to
            var oldRoleName = oldRoleNameList.FirstOrDefault(); // gets the first element of the list, there should only be one role per user

            if (!string.IsNullOrEmpty(oldRoleName) && !oldRoleName.Equals(roleName))
            {
                await _accountRepository.RemoveFromRoleAsync(user, oldRoleName, ct);
                await _accountRepository.AddToRoleAsync(user, roleName, ct);

                await _accountRepository.UpdateAsync(user, ct);
            }
        }

        public void SendEmail(string Email, string Code, string Purpose, string scheme, HostString host)
        {
            var fromEmail = new MailAddress("noreplyVIPS@gmail.com", "VIPS"); // change email from mine
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "zatp cpuw nbla owjp";

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

        public string ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return "Email is empty";
            }

            if (!email.Contains("@unf.edu"))
            {
                return "Email must contain \"@unf.edu\"";
            }

            return "";
        }

        public string ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return "Password must not be empty";
            }

            return "";
        }
        public string ValidatePassword(string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                return "Password must not be empty";
            }
            if (!password.Equals(confirmPassword))
            {
                return "Passwords do not match";
            }

            return "";
        }
    }
}
