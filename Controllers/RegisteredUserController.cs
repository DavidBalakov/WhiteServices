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

        //change role method
        //index method (to list all users)
        //create worker
    }
}