using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Helpers;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Models.ViewModels;
using AspNetCoreDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDemo.Controllers.MVC
{
    public class AuthenticationController : Controller
    {
        private readonly AuthManager _authManager;
        private readonly IUsersService _usersService;
        public AuthenticationController(AuthManager authManager, IUsersService usersService)
        {
            _authManager = authManager;
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel) {
            
            if(!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            string credentials = loginViewModel.Username + ":" + loginViewModel.Password;

            try
            {
                var user = _authManager.TryGetUser(credentials);
                HttpContext.Session.SetString("CurrentUser", user.Username);
                return RedirectToAction("Index", "Home");
            }
            catch (UnauthorizedOperationException e)
            {
                ModelState.AddModelError("Username", e.Message);
                ModelState.AddModelError("Password",e.Message);
                return View(loginViewModel);
            }
        }

        [HttpGet]
        public IActionResult Logout() {
            HttpContext.Session.Remove("CurrentUser");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var user = new User { Username = registerViewModel.Username, Password = registerViewModel.Password };
                    _usersService.CreateUser(user);
                    HttpContext.Session.SetString("CurrentUser", user.Username);
                    return RedirectToAction("Index", "Home");
                }
                catch (DuplicateEntityException e)
                {
                    ModelState.AddModelError("Username", e.Message);
                    return View(registerViewModel);
                }

            }
            return View(registerViewModel);
        }
    }
}
