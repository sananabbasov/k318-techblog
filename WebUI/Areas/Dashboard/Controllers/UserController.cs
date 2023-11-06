using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Areas.Dashboard.ViewModels;
using WebUI.Models;

namespace WebUI.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    public async Task<IActionResult> AddRole(string id)
    {
        var roles = _roleManager.Roles.ToList();
        var user = await _userManager.FindByIdAsync(id);

        UserRoleVM userVm = new()
        {
            Roles = roles,
            User = user
        };
        return View(userVm);
    }


    [HttpPost]
    public async Task<IActionResult> AddRole(string roleName, string userId)
    {
        var findUser = await _userManager.FindByIdAsync(userId);
        var userRoles = await _userManager.GetRolesAsync(findUser);
        await _userManager.RemoveFromRolesAsync(findUser, userRoles);
        var addRole = await _userManager.AddToRoleAsync(findUser, roleName);

        if (addRole.Succeeded)
        {
            return RedirectToAction("Index");
        }
        return View();
    }

}
