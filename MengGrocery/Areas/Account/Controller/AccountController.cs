using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MengGrocery.DAL;
using MengGrocery.Helpers;
using MengGrocery.Models;
using MengGrocery.Migrations;



    [Area("Account")]
    
    public class AccountController : Controller
    {
        private readonly CustomUserManager _userManager;
        private readonly CustomSignInManager _signInManager;
        private readonly IAccountQuery _accountQuery;

        public AccountController(CustomUserManager userManager, CustomSignInManager signInManager, IAccountQuery accountQuery)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountQuery = accountQuery;
        }

        [HttpGet]
        //this action require login
        [Authorize]
        public IActionResult Index()
        {
            //Check if logged
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("login", "account", new { area = "account" });
            }

            var email = User.FindFirstValue(ClaimTypes.Email);

            var orders = _accountQuery.GetRecentOrders(email);

            var model = new AccountViewModel
            {
                Orders = orders
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home", new { area = "" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewData["ReturnUrl"] = model.ReturnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {

              

            var user = await _userManager.FindByEmailAsync(model.Email);
            var p = await _signInManager.CreateUserPrincipalAsync(user);
          



var t = _signInManager.IsSignedIn(p);


                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home", new { area = "" });

                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home", new { area = ""});
        }
    }

