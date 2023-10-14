using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Data;
using WebUI.Dtos;
using WebUI.Models;

namespace WebUI.ViewComponents;

public class AuthViewComponent : ViewComponent
{

    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContext;
    private readonly UserManager<User> _userManager;

    public AuthViewComponent(AppDbContext context, IHttpContextAccessor httpContext, UserManager<User> userManager)
    {
        _context = context;
        _httpContext = httpContext;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = await _userManager.FindByIdAsync(userId);
        UserInfoDto userInfo = new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhotoUrl = user.PhotoUrl
        };
        return View("Auth",userInfo);
    }
}
