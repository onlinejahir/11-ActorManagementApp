using _11_ActorManagementApp.ProjectModels.Contracts;
using _11_ActorManagementApp.ViewModels.IdentityVM;
using ActorManagement.Models.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _11_ActorManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILoggedInUserInfo _loggedInUserInfo;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILoggedInUserInfo loggedInUserInfo)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._loggedInUserInfo = loggedInUserInfo;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVm)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    FirstName = registerVm.FirstName,
                    LastName = registerVm.LastName,
                    DateOfBirth = registerVm.DateOfBirth,
                    UserName = registerVm.UserName,
                    Email = registerVm.Email
                };
                var result = await _userManager.CreateAsync(user, registerVm.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Create", "Actor");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(registerVm);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)

        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, loginVM.RememberMe, false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl ?? Url.Content("~/"));
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(loginVM);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (ModelState.IsValid)
            {
                var userId = _loggedInUserInfo.GetUserId();
                var user = await _userManager.FindByIdAsync(userId);
                var result = await _userManager.ChangePasswordAsync(user, changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(changePasswordVM);
        }
    }
}
