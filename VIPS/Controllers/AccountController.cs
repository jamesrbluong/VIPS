﻿using Microsoft.AspNet.Identity;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using VIPS.Models.ViewModels.Account;
using VIPS.Models.ViewModels.Account.ForgotPassword;
using System.Threading;
using Services.Account;
using Services.Contracts;
using Repositories.Accounts;

namespace VIPS.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountService accountService, IAccountRepository accountRepository)
        {
            _accountService = accountService;
            _accountRepository = accountRepository;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var temp = await _accountRepository.GetListAsync(ct);

            var model = new IndexViewModel
            {
                UserList = temp,
                AccountTotal = temp.Count()
            };
            return View(model);
        }

        public async Task<IActionResult> SearchAccounts (string query, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var temp = await _accountService.SearchAccounts(query, ct);

                var model = new IndexViewModel
                {
                    UserList = temp,
                    AccountTotal = temp.Count()
                };
                return View("Index", model);
            }

            return RedirectToAction("Index");
            
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl // TODO: make returnUrl actually work
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccount(LoginViewModel model, CancellationToken ct)
        {
            string validateEmailString = _accountService.ValidateEmail(model.Email);
            string validatePasswordString = _accountService.ValidatePassword(model.Password);
            if (!string.IsNullOrEmpty(validateEmailString))
            {
                TempData["error"] = validateEmailString;
                return RedirectToAction("Create", "Account");
            }
            else if (!string.IsNullOrEmpty(validatePasswordString))
            {
                TempData["error"] = validatePasswordString;
                return RedirectToAction("Create", "Account");
            }

            AppUser user = await _accountRepository.GetByEmailAsync(model.Email, ct);

            if (user == null)
            {
                TempData["error"] = "Invalid Email or Password";
                return View("Login", model);
            }

            await _accountRepository.SignOutAsync(ct);
            if (_accountRepository.PasswordSignInAsync(user, model.Password, false, true, ct).Result.Succeeded) // compare to old code
            {
                var roleNameList = await _accountRepository.GetRolesAsync(user, ct); // each user only has one role

                HttpContext.Session.SetString("CurrentEmail", user.Email);
                HttpContext.Session.SetString("CurrentUserRole", roleNameList.FirstOrDefault());

                TempData["success"] = user.Email + " logged in successfully!";

                return RedirectToAction("Index", "Home");
            }
            else if (_accountRepository.PasswordSignInAsync(user, model.Password, false, true, ct).Result.IsLockedOut)
            {
                TempData["error"] = "Too many failed login attempts. Try again later.";
            }
            else
            {
                TempData["error"] = "Invalid Email or Password";
            }

            return View("Login", model);
            
        }

        [AllowAnonymous]
        public async Task<RedirectResult> Logout(string returnUrl, CancellationToken ct)
        {
            await _accountRepository.SignOutAsync(ct);
            HttpContext.Session.SetString("CurrentEmail", string.Empty); // Empty Session Strings
            HttpContext.Session.SetString("CurrentUserRole", string.Empty);


            returnUrl = "/";
            return Redirect(returnUrl);
        }

        // [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount(CreateViewModel model, CancellationToken ct)
        {
            string validateEmailString = _accountService.ValidateEmail(model.Email);
            string validatePasswordString = _accountService.ValidatePassword(model.Password, model.ConfirmPassword);
            if (!string.IsNullOrEmpty(validateEmailString))
            {
                TempData["error"] = validateEmailString;
                return RedirectToAction("Create", "Account");
            }
            else if (!string.IsNullOrEmpty(validatePasswordString))
            {
                TempData["error"] = validatePasswordString;
                return RedirectToAction("Create", "Account");
            }

            AppUser user = new AppUser // Create user with values from the model
            {
                UserName = model.Email,
                Email = model.Email
            };

            Microsoft.AspNetCore.Identity.IdentityResult result = await _accountRepository.CreateAccountAsync(user, model.Password, ct);

            if (result.Succeeded)
            {
                await _accountRepository.AddToRoleAsync(user, "UNF_Employee", ct);

                LoginViewModel temp = new LoginViewModel
                {
                    Email = model.Email,
                    Password = model.Password
                };

                TempData["success"] = "Account created successfully";
                return await LoginAccount(temp, ct);

            }
            else
            {
                TempData["error"] = "Error occurred, account was not created";
                return RedirectToAction("Create", "Account");
            }
        }

        

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            var currUser = await _accountRepository.GetCurrentUser(User.Identity.GetUserId(), ct);
            AppUser user = await _accountRepository.GetByIdAsync(id, ct);

            if (currUser.Id.Equals(user.Id))
            {
                TempData["error"] = "You cannot delete the account you are currently logged in to";
                return RedirectToAction("Index", "Account");
            }

            if (user != null)
            {
                Microsoft.AspNetCore.Identity.IdentityResult result = await _accountRepository.DeleteAccountAsync(user, ct);
                if (result.Succeeded)
                {
                    TempData["success"] = "User deleted successfully";
                    return RedirectToAction("Index", "Account");
                }

            }
            else
            {
                TempData["error"] = "User not found";
            }

            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, CancellationToken ct)
        {
            var currUser = await _accountRepository.GetCurrentUser(User.Identity.GetUserId(), ct);
            AppUser oldUser = await _accountRepository.GetByIdAsync(id, ct);

            if (currUser.Id.Equals(oldUser.Id))
            {
                TempData["error"] = "You cannot edit the account you are currently logged in to";
                return RedirectToAction("Index", "Account");
            }

            if (oldUser != null)
            {
                var roleNameList = await _accountRepository.GetRolesAsync(oldUser, ct);

                var roleName = roleNameList.FirstOrDefault();

                var model = new EditViewModel
                {
                    Id = id,
                    SecurityStamp = oldUser.SecurityStamp,
                    RoleName = roleName
                };

                return View(model);
            }

            TempData["error"] = "User not found";
            return RedirectToAction("Index", "Account");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAccount(EditViewModel model, CancellationToken ct)
        {
            AppUser user = await _accountRepository.GetByIdAsync(model.Id, ct);
            if (user != null)
            {
                if (user == null || string.IsNullOrEmpty(model.RoleName))
                {
                    TempData["error"] = "No change to user role";
                    return View("EditAccount", model.Id);
                }

                TempData["success"] = "Role for user has been updated";
                await _accountService.ChangeRole(model.RoleName, user, ct);

            }
            else
            {
                TempData["error"] = "User not found";
            }
            
            return RedirectToAction("Index", "Account");

        }


        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordSendEmail(ForgotPasswordViewModel model, CancellationToken ct)
        {
            string Email = model.Email;
            AppUser user = await _accountRepository.GetByEmailAsync(Email, ct);
            if (user != null)
            {
                // Send Email
                await _accountRepository.UpdateSecurityStampAsync(user, ct);
                string resetCode = await _accountRepository.GeneratePasswordResetTokenAsync(user, ct); // Guid.NewGuid().ToString();
                _accountService.SendEmail(user.Email, resetCode, "ResetPassword", HttpContext.Request.Scheme, HttpContext.Request.Host);

                await _accountRepository.UpdateAsync(user, ct);
                

            }

            TempData["success"] = "Email with further instructions was sent successfully";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code, string email) 
        {
            var model = new ResetPasswordViewModel
            {
                Email = email,
                ResetCode = code
            };

            return View(model);
            
        }

        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordUpdate(ResetPasswordViewModel model, CancellationToken ct)
        {
            string validatePasswordString = _accountService.ValidatePassword(model.NewPassword, model.ConfirmPassword);
            if (!string.IsNullOrEmpty(validatePasswordString))
            {
                return RedirectToAction("ResetPassword", "Account", new
                {
                    code = model.ResetCode,
                    email = model.Email
                });
            }
            if (!string.IsNullOrEmpty(model.Email)) 
            {
                var user = await _accountRepository.GetByEmailAsync(model.Email, ct); // add email form input to view and get from view, search user for email
                if (user != null)
                {
                    if (await _accountRepository.HasPasswordAsync(user, ct))
                    {
                        // Console.WriteLine("test" + model.Email + model.NewPassword + model.ResetCode);

                        Microsoft.AspNetCore.Identity.IdentityResult result = await _accountRepository.ResetPasswordAsync(user, model.ResetCode, model.NewPassword, ct);
                        if (result.Succeeded)
                        {
                            TempData["success"] = "Password updated successfully";
                            return RedirectToAction("Login", "Account");
                        }
                        else
                        {
                            TempData["error"] = "Reset Password Token could not be verified";
                        }
                    }
                    else
                    {
                        TempData["error"] = "User account has no password";
                    }
                    // await _userManager.UpdateSecurityStampAsync(user);
                }
                else
                {
                    TempData["error"] = "User not found";
                }
            }

            

            return RedirectToAction("ForgotPassword", "Account");
        }

        
    }
}
