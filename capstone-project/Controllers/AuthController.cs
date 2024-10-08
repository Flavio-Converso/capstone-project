﻿using capstone_project.Interfaces;
using capstone_project.Interfaces.Auth;
using capstone_project.Models.DTOs.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace capstone_project.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ICategoryService _categorySvc;

        public AuthController(IAuthService authService, ICategoryService categoryService)
        {
            _authService = authService;
            _categorySvc = categoryService;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect authenticated users to another page, like the home or dashboard
                return RedirectToAction("List", "Game");
            }
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                var existingUser = await _authService.LoginAsync(dto);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.Username),
                    new Claim(ClaimTypes.NameIdentifier, existingUser.UserId.ToString())
                };

                existingUser.Roles.ForEach(r =>
                {
                    claims.Add(new Claim(ClaimTypes.Role, r.Name));
                });

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("List", "Game");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("LoginError", "Tentativo di login fallito: " + ex.Message);
                return View(dto);
            }
        }

        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect authenticated users to another page, like the home or dashboard
                return RedirectToAction("List", "Game");
            }
            var categories = await _categorySvc.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categorySvc.GetAllCategoriesAsync();
                ViewBag.Categories = categories;
                return View(dto);
            }

            try
            {
                await _authService.RegisterAsync(dto);

                // Store a message in TempData
                TempData["SuccessMessage"] = "Registrazione completata con successo! Effettua il login per continuare.";

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                var categories = await _categorySvc.GetAllCategoriesAsync();
                ViewBag.Categories = categories;
                ModelState.AddModelError("RegisterError", "Registrazione fallita: " + ex.Message);
                return View(dto);
            }
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}


