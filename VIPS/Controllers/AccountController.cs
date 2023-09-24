using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VIPS.Models.Data;
using VIPS.Models.Account;
using Microsoft.IdentityModel.Tokens;

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

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginAccountModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccount(LoginAccountModel model)
        {
            if (!model.Email.IsNullOrEmpty())
            {
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    if ((await _signInManager.PasswordSignInAsync(user, model.Password, false, false)).Succeeded)
                    {
                        HttpContext.Session.SetString("CurrentUserName", user.UserName);

                        var roleNameList = await _userManager.GetRolesAsync(user); // each user only has one role
                        if (roleNameList != null)
                        {
                            TempData["success"] = user.UserName + " logged in successfully!";

                            /*
                            if (roleNameList.FirstOrDefault().Equals("Admin"))
                            {
                                return RedirectToAction("Index", "Account");
                            }
                            */
                        }

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
            HttpContext.Session.SetString("CurrentUserName", String.Empty);
            return Redirect(returnUrl);
        }


    }
}
