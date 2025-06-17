using _11_ActorManagementApp.EmailServices.Contracts;
using _11_ActorManagementApp.ViewModels.IdentityVM;
using ActorManagement.Models.EntityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _11_ActorManagementApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IEmailService emailService)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._emailService = emailService;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleVM createRoleVm)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = createRoleVm.RoleName
                };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(createRoleVm);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id: {id} cannot be found";
                return View("_NotFoundPartial");
            }
            var editRoleVm = new EditRoleVM()
            {
                Id = role.Id,
                RoleName = role.Name
            };
            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    editRoleVm.Users.Add(user.UserName);
                }
            }
            return View(editRoleVm);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleVM editRoleVm)
        {
            var role = await _roleManager.FindByIdAsync(editRoleVm.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id: {editRoleVm.Id} cannot be found";
                return View("_NotFoundPartial");
            }
            else
            {
                role.Name = editRoleVm.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(editRoleVm);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.RoleId = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role Id: {roleId} cannot be found";
                return View("_NotFoundPartial");
            }
            List<UserRoleViewModel> userRoleViewModels = new List<UserRoleViewModel>();
            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                userRoleViewModels.Add(userRoleViewModel);
            }
            return View(userRoleViewModels);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> userRoleVMs, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id: {roleId} cannot be found";
                return View("_NotFoundPartial");
            }
            for (var i = 0; i < userRoleVMs.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(userRoleVMs[i].UserId);
                IdentityResult result = null!;
                if (userRoleVMs[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!(userRoleVMs[i].IsSelected) && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (userRoleVMs.Count - 1))
                    {
                        continue;
                    }
                    else RedirectToAction("EditRole", "Admin", new { id = roleId });
                }
            }
            return RedirectToAction("EditRole", "Admin", new { id = roleId });
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendBulkEmail()
        {
            List<string> recipients = new List<string>(){
                "contact.jahir24@gmail.com",
                "contact.jahirau@gmail.com",
                "document.jahir@gmail.com"
            };

            string subject = await System.IO.File.ReadAllTextAsync("wwwroot/EmailTemplates/EmailSubject.txt");
            string body = await System.IO.File.ReadAllTextAsync("wwwroot/EmailTemplates/EmailTemplate.html");

            bool sent = await _emailService.SendBulkEmailAsync(recipients, subject, body);

            TempData["Message"] = sent
                ? "All email sent successfully!"
                : "Something went wrong while sending bulk email";

            return RedirectToAction("Dashboard", "Admin");
        }

        public IActionResult EmailDashboard()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
    }
}
