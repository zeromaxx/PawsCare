using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawsCare.Data;
using PawsCare.Models;
using System.Security.Claims;
using PawsCare.Models.ViewModels;

namespace PawsCare.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
             HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Store(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", user);
            }

            var existingUser =  _context.Users
                                             .FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "A user with this email already exists.");
                return View("Register", user);
            }
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);

            _context.Users.Add(user);
             _context.SaveChanges();

            ViewBag.SuccessMessage = "Your details have been saved successfully!";

            return View("Register", user);
        }
        public IActionResult StartSession(LoginViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", viewmodel);
            }

            var user =  _context.Users
                        .FirstOrDefault(user => user.Email == viewmodel.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "No account found with that email address.");
                return View("Login", viewmodel);
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, viewmodel.Password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "The credentials you provided, do not match our records.");
                return View("Login", viewmodel);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role),

            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };
             HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }

    }
}
