using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebUI.Data;
using WebUI.Models;

namespace WebUI.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize(Roles ="Admin, Moderator")]
public class TagController : Controller
{
    private readonly AppDbContext _context;

    public TagController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var tags = _context.Tags.Include(x=>x.ArticleTags).ThenInclude(x=>x.Article).ToList();
        return View(tags);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Create(string tagName)
    {
        if (string.IsNullOrWhiteSpace(tagName))
            return View();

        var findTag = _context.Tags.FirstOrDefault(x=>x.TagName == tagName);
        if (findTag != null)
            return View();
            
        Tag tag = new()
        {
            TagName = tagName,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };
        _context.Tags.Add(tag);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

}
