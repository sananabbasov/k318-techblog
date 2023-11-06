using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Dtos;
using WebUI.Models;

namespace WebUI.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IHttpContextAccessor _httpContext;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContext, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContext = httpContext;
        _roleManager = roleManager;
    }
    public IActionResult Login()
    {
        var user = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (user != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {


        var findUser = await _userManager.FindByEmailAsync(loginDto.Email);
        if (findUser == null)
            return View();


        var result = await _signInManager.PasswordSignInAsync(findUser, loginDto.Password, true, true);
        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        return View();
    }
    //GET, POST
    //AOP

    [HttpGet] // Attribute
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var findUser = await _userManager.FindByEmailAsync(registerDto.Email);

        if (findUser != null)
            return View();

        User newUser = new()
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
            PhotoUrl = "/",
            About = " ",
            ConfirmationToken = Guid.NewGuid().ToString()
        };

        IdentityResult result = await _userManager.CreateAsync(newUser, registerDto.Password);


        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, "User");
            return RedirectToAction("Login");
        }


        return View();
    }

    public IActionResult Forgot()
    {
        return View();
    }

    public IActionResult Logout()
    {

        _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}
