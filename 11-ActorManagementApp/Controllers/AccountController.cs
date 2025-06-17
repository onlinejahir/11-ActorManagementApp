using _11_ActorManagementApp.EmailServices.Contracts;
using _11_ActorManagementApp.ProjectModels.Contracts;
using _11_ActorManagementApp.ViewModels.IdentityVM;
using ActorManagement.Models.EntityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace _11_ActorManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILoggedInUserInfo _loggedInUserInfo;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILoggedInUserInfo loggedInUserInfo, IEmailService emailService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._loggedInUserInfo = loggedInUserInfo;
            this._emailService = emailService;
        }
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM registerVm)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    FirstName = registerVm.FirstName,
                    LastName = registerVm.LastName,
                    DateOfBirth = registerVm.DateOfBirth,
                    UserName = registerVm.Email,
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
        [HttpGet]
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
        [HttpGet]
        [Authorize]
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendEmail(EmailViewModel emailVM)
        {
            if (ModelState.IsValid)
            {
                string adminEmail = "contact.jahir24@gmail.com";
                //Build email body
                string body = $@"<p><strong>Sender Name: </strong>{emailVM.PersonName}</p> 
                                 <p><strong>Sender Email: </strong>{emailVM.PersonEmail}</p> 
                                 <p><strong>Message: </strong>{emailVM.Message}</p>";

                //Send email
                bool emailSent = await _emailService.SendEmailAsync(adminEmail, emailVM.Subject, body);

                if (emailSent)
                {
                    ModelState.Clear();
                    ViewBag.Message = "Your message has been sent successfully";
                    return View(new EmailViewModel());
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Sorry, there was an error sending your message. Please try again later");
                }
            }
            return View(emailVM);
        }
    }
}
