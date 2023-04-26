using ISApplication.Data;
using ISApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ISApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly SignInManager<IdentityUser> _SignInManager;

        public AccountController(UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> signInManager)
        {
            _UserManager = UserManager;
            _SignInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (_SignInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
            if (_SignInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var User = new IdentityUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var Result = await _UserManager.CreateAsync(User, model.Password);

                if (Result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View("Register", model);
        }

        [HttpGet]
        public IActionResult Login(string? ReturnURL)
        {
            if (_SignInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            ViewData["ReturnURL"] = ReturnURL;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnURL)
        {
            if (_SignInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
              var Result = await _SignInManager.PasswordSignInAsync
                    (model.UserName, model.Password, model.Rememberme, true);


                if (Result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnURL) && Url.IsLocalUrl(ReturnURL))
                    return Redirect(ReturnURL);
                   

                    return RedirectToAction("Index", "Home");
                }

                if (Result.IsLockedOut)
                {
                    ViewData["ErrorMessage"] = "اکانت شما به مدت 5 دقیقه قفل شده است";
                    return View("Login" , model);
                }

                ModelState.AddModelError(string.Empty, "رمز عبور یا نام کاربری اشتباه است");
            }
            return View("Login", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOut()
        {
            _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}
