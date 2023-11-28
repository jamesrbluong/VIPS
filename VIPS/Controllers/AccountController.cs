using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VIPS.Models.Data;
using VIPS.Models.Account;


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
        public ViewResult Index()
        {
            var model = new AccountIndexModel
            {
                UserList = _userManager.Users
            };

            return View(model);
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginAccountModel
            {
                ReturnUrl = returnUrl // TODO: make returnUrl actually work
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccount(LoginAccountModel model)
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
            HttpContext.Session.SetString("CurrentEmail", String.Empty);
            HttpContext.Session.SetString("CurrentUserRole", String.Empty);

            return Redirect(returnUrl);
        }

        // [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(LoginAccountModel model)
        {
            AppUser user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //await signInManager.SignInAsync(user, isPersistent: false); 

                

                TempData["success"] = "Account created successfully";
                await _userManager.AddToRoleAsync(user, "UNF_Employee");
                return await LoginAccount(model);

            }

            TempData["error"] = "Error occurred, account was not created";
            return RedirectToAction("Index", "Home");
        }


    }
}
