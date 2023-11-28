// using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;
using VIPS.Models.Data;
using VIPS.Models.ViewModels.Account;
using VIPS.Models.ViewModels.Account.ForgotPassword;

namespace VIPS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var model = new IndexViewModel
            {
                UserList = _userManager.Users
            };

            return View(model);
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
        public async Task<IActionResult> LoginAccount(LoginViewModel model)
        {
            if (!model.Email.IsNullOrEmpty()) // if the email is not empty
            {
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null) // if the user associated with the email exists
                {
                    await _signInManager.SignOutAsync(); // sign out current user if they are signed in
                    if ((await _signInManager.PasswordSignInAsync(user, model.Password, false, false)).Succeeded)
                    {
                        var roleNameList = await _userManager.GetRolesAsync(user); // each user only has one role

                        HttpContext.Session.SetString("CurrentEmail", user.Email);
                        HttpContext.Session.SetString("CurrentUserRole", roleNameList.FirstOrDefault());

                        TempData["success"] = user.UserName + " logged in successfully!";

                        Console.WriteLine(roleNameList.FirstOrDefault());

                        return RedirectToAction("Index", "Home");
                    }

                }
            }

            TempData["error"] = "Invalid Email or Password";
            return View("Login", model);
        }

        [AllowAnonymous]
        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.SetString("CurrentEmail", String.Empty); // Empty Session Strings
            HttpContext.Session.SetString("CurrentUserRole", String.Empty);

            return Redirect(returnUrl);
        }

        // [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount(LoginViewModel model)
        {
            if (true) // model.Email.Contains("@unf.edu")
            {
                AppUser user = new AppUser // Create user with values from the model
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    TempData["success"] = "Account created successfully";
                    await _userManager.AddToRoleAsync(user, "UNF_Employee");
                    return await LoginAccount(model);

                }
                else
                {
                    TempData["error"] = "Error occurred, account was not created";
                }
            }
            else
            {
                TempData["error"] = "Error occurred, email must contain @unf.edu";
            }

            return RedirectToAction("Create", "Account");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
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
        public async Task<IActionResult> Edit(string id)
        {
            AppUser oldUser = await _userManager.FindByIdAsync(id);
            if (oldUser != null)
            {
                var roleNameList = await _userManager.GetRolesAsync(oldUser);

                var roleName = roleNameList.FirstOrDefault();

                var model = new EditViewModel
                {
                    Id = id,
                    SecurityStamp = oldUser.SecurityStamp,
                    Email = oldUser.Email,
                    RoleName = roleName
                };

                return View(model);
            }

            TempData["error"] = "User not found";
            return RedirectToAction("Index", "Account");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAccount(EditViewModel model)
        {
            // Console.WriteLine("Test1 " + model.NewUserName + model.NewUserEmail);
            AppUser oldUser = await _userManager.FindByIdAsync(model.Id);
            if (oldUser != null)
            {
                oldUser.Email = model.Email;

                await ChangeRole(model);

                await _userManager.UpdateAsync(oldUser);

                TempData["success"] = "User has been successfully edited";
                return RedirectToAction("Index", "Account");
            }

            TempData["error"] = "User not found";
            return RedirectToAction("Index", "Account");

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(EditViewModel model)
        {
            // Console.WriteLine("Test: " + model.Id + " AND " + model.RoleName);
            AppUser user = await _userManager.FindByIdAsync(model.Id); // gets the user whos role is being changed

            if (user != null)
            {
                var oldRoleNameList = await _userManager.GetRolesAsync(user); // gets a list of all the roles the user is assigned to
                var oldRoleName = oldRoleNameList.FirstOrDefault(); // gets the first element of the list, there should only be one role per user

                if ((oldRoleName != null) && !oldRoleName.Equals(model.RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(user, oldRoleName);
                    TempData["success"] = "Role for user: " + model.Email + " has been updated from " + oldRoleName + " to " + model.RoleName;
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    return RedirectToAction("Index", "Account");
                }

            }
            TempData["error"] = "No change to user role";
            return RedirectToAction("Index", "Account");
        }
        

        public void SendEmail(string Email, string Code, string Purpose)
        {
            var VerifyUrl = "/Account/" + Purpose + "/" + Code;
            var link = "www.google.com"; // update url

            var fromEmail = new MailAddress("joshuastabile@gmail.com", "test"); // change email from mine
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "mdlaxyzndlimjlql";

            string subject = "";
            string body = "";

            if (Purpose == "ForgotPassword")
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
                UseDefaultCredentials = true,
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

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordSendEmail(ForgotPasswordViewModel model)
        {
            string Email = model.Email;
            AppUser user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                // Send Email
                string resetCode = Guid.NewGuid().ToString();
                SendEmail(user.Email, resetCode, "ForgotPassword");
                // user.ResetPasswordCode = resetCode;

                await _userManager.UpdateAsync(user);

            }
            else
            {
                TempData["error"] = "User not found";

            }

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel();

                model.ResetCode = id;
                return View(model);

            }

            TempData["error"] = "User not found";
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.ResetCode);
            if (user != null)
            {
                if (await _userManager.HasPasswordAsync(user))
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, model.NewPassword);

                    TempData["success"] = "Password updated successfully";
                    return RedirectToAction("Index", "Home");
                }

                

            }

            TempData["error"] = "User not found";
            return RedirectToAction("Index", "Home");
        }

        
    }
}
