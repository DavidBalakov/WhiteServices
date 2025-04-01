using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Diploma.Services.Register;
using Microsoft.AspNetCore.Authorization;
using Diploma.Models.ViewModels;

namespace Products.Controllers
{
    public class RegisteredUserController : Controller
    {
        private readonly ILogisterService _logisterService;
        public RegisteredUserController(ILogisterService logisterService)
        {
            this._logisterService = logisterService;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerRequest)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _logisterService.Register(registerRequest);
                if (result.Succeeded)
                {
                    TempData["SuccessLogin"] = "Регистрацията е успешна.";
                    return RedirectToAction("Index", "Home");
                }
                TempData["FailedLogin"] = "Възникна грешка!";
            }
            return View(registerRequest);
        }
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel logInRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _logisterService.LogIn(logInRequest);
                if (result.Succeeded)
                {
                    TempData["SuccessLogin"] = "Влизането в акаунта е успешно.";
                    return RedirectToAction("Index", "Home");
                }
                TempData["FailedLogin"] = "Възникна грешка!";
            }
            return View(logInRequest);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            TempData["SuccessLogin"] = "(Излезе от акаунта).";
            await _logisterService.LogOut();
            return RedirectToAction("Index", "Home");
        }

        // Change role method
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newRole))
            {
                TempData["FailedAction"] = "Невалидни данни за промяна на роля.";
                return RedirectToAction("Index");
            }

            var result = await _logisterService.ChangeUserRole(userId, newRole);
            if (result.Succeeded)
            {
                TempData["SuccessAction"] = "Ролята е променена успешно.";
            }
            else
            {
                TempData["FailedAction"] = "Възникна грешка при промяна на ролята.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _logisterService.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateWorker()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateWorker(RegisterViewModel registerRequest)
        {
            if (ModelState.IsValid)
            {
                registerRequest.Role = "Technician";

                IdentityResult result = await _logisterService.Register(registerRequest);
                if (result.Succeeded)
                {
                    TempData["SuccessAction"] = "Работникът е регистриран успешно.";
                    return RedirectToAction("Index");
                }
                TempData["FailedAction"] = "Възникна грешка при регистрацията на работника.";
            }
            return View(registerRequest);
        }
    }
}