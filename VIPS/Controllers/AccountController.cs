// using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using VIPS.Models.Data;
using VIPS.Models.ViewModels.Account;
using VIPS.Models.ViewModels.Account.ForgotPassword;

namespace VIPS.Controllers
{
    public class AccountController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IHttpContextAccessor _accessor;

        public AccountController(Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole<Guid>> roleManager, IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _accessor = accessor;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var model = new IndexViewModel
            {
                UserList = _userManager.Users,
                AccountTotal = _userManager.Users.Count()
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
            if (ValidateEmail(model.Email) == false || ValidatePassword(model.Password) == false)
            {
                return View("Login", model);
            }

            AppUser user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                TempData["error"] = "Invalid Email or Password";
                return View("Login", model);
            }

            await _signInManager.SignOutAsync(); // sign out current user if they are signed in
            if ((await _signInManager.PasswordSignInAsync(user, model.Password, false, true)).Succeeded)
            {
                var roleNameList = await _userManager.GetRolesAsync(user); // each user only has one role

                HttpContext.Session.SetString("CurrentEmail", user.Email);
                HttpContext.Session.SetString("CurrentUserRole", roleNameList.FirstOrDefault());

                TempData["success"] = user.Email + " logged in successfully!";

                return RedirectToAction("Index", "Home");
            }
            else if ((await _signInManager.PasswordSignInAsync(user, model.Password, false, true)).IsLockedOut)
            {
                TempData["error"] = "Too many failed login attempts. Try again later.";
            }
            else
            {
                TempData["error"] = "Invalid Email or Password";
            }

            return View("Login", model);
            
        }

        

        public bool ValidateEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                TempData["error"] = "Email must not be empty";
                return false;
            }

            if (!email.Contains("@unf.edu"))
            {
                TempData["error"] = "Email must contain \"@unf.edu\"";
                return false;
            }

            return true;
        }

        public bool ValidatePassword(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                TempData["error"] = "Password must not be empty";
                return false;
            }

            return true;
        }
        public bool ValidatePassword(string password, string confirmPassword) 
        {
            if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(confirmPassword))
            {
                TempData["error"] = "Password must not be empty";
                return false;
            }
            if (!password.Equals(confirmPassword))
            {
                TempData["error"] = "Passwords do not match";
                return false;
            }

            return true;
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
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount(CreateViewModel model)
        {
            if (ValidateEmail(model.Email) == false || ValidatePassword(model.Password, model.ConfirmPassword) == false)
            {
                return RedirectToAction("Create", "Account");
            }

            AppUser user = new AppUser // Create user with values from the model
            {
                UserName = model.Email,
                Email = model.Email
            };

            Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                TempData["success"] = "Account created successfully";
                await _userManager.AddToRoleAsync(user, "UNF_Employee");

                LoginViewModel temp = new LoginViewModel
                {
                    Email = model.Email,
                    Password = model.Password
                };

                return await LoginAccount(temp);

            }
            else
            {
                TempData["error"] = "Error occurred, account was not created";
                return RedirectToAction("Create", "Account");
            }
        }

        

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var currUser = await _userManager.FindByIdAsync(User.Identity.GetUserId());

            if (currUser.Id.ToString().Equals(id))
            {
                TempData["error"] = "You cannot delete the account you are currently logged in to";
                return RedirectToAction("Index", "Account");
            }

            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.DeleteAsync(user);
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
            var currUser = await _userManager.FindByIdAsync(User.Identity.GetUserId());

            if (currUser.Id.ToString().Equals(id))
            {
                TempData["error"] = "You cannot edit the account you are currently logged in to";
                return RedirectToAction("Index", "Account");
            }

            AppUser oldUser = await _userManager.FindByIdAsync(id);

            if (oldUser != null)
            {
                var roleNameList = await _userManager.GetRolesAsync(oldUser);

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
        public async Task<IActionResult> EditAccount(EditViewModel model)
        {
            AppUser user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                await ChangeRole(model, user);
            }
            else
            {
                TempData["error"] = "User not found";
            }
            
            return RedirectToAction("Index", "Account");

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(EditViewModel model, AppUser user)
        {
            if (user == null || string.IsNullOrEmpty(model.RoleName))
            {
                TempData["error"] = "No change to user role";
                return View("EditAccount", model.Id);
            }

            var oldRoleNameList = await _userManager.GetRolesAsync(user); // gets a list of all the roles the user is assigned to
            var oldRoleName = oldRoleNameList.FirstOrDefault(); // gets the first element of the list, there should only be one role per user

            if (!string.IsNullOrEmpty(oldRoleName) && !oldRoleName.Equals(model.RoleName))
            {
                await _userManager.RemoveFromRoleAsync(user, oldRoleName);
                await _userManager.AddToRoleAsync(user, model.RoleName);

                await _userManager.UpdateAsync(user);

                TempData["success"] = "Role for user has been updated from " + oldRoleName + " to " + model.RoleName;
                
            }

            return RedirectToAction("Index", "Account");

        }

        
        public void SendEmail(string Email, string Code, string Purpose)
        {
            var fromEmail = new MailAddress("joshuastabile@gmail.com", "test"); // change email from mine
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "gynn cppj sxpk bxbc";

            string baseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);

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

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordSendEmail(ForgotPasswordViewModel model)
        {
            string Email = model.Email;
            AppUser user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                // Send Email
                await _userManager.UpdateSecurityStampAsync(user);
                string resetCode = await _userManager.GeneratePasswordResetTokenAsync(user); // Guid.NewGuid().ToString();
                SendEmail(user.Email, resetCode, "ResetPassword");

                await _userManager.UpdateAsync(user);
                

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
        public async Task<IActionResult> ResetPasswordUpdate(ResetPasswordViewModel model)
        {
            if (ValidatePassword(model.NewPassword, model.ConfirmPassword) == false)
            {
                return RedirectToAction("ResetPassword", "Account", new
                {
                    code = model.ResetCode,
                    email = model.Email
                });
            }

            var user = await _userManager.FindByEmailAsync(model.Email); // add email form input to view and get from view, search user for email
            if (user != null)
            {
                if (await _userManager.HasPasswordAsync(user))
                {
                    // Console.WriteLine("test" + model.Email + model.NewPassword + model.ResetCode);

                    Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.ResetPasswordAsync(user, model.ResetCode, model.NewPassword);
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

            return RedirectToAction("ForgotPassword", "Account");
        }

        
    }
}
