using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Data;
using WebUI.Models;
using WebUI.ViewModels;

namespace WebUI.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(int pageNo = 1)
    {
        string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        int postsPerPage = 3; // TODO: Make this a setting in the database or something
        int skipPage = (pageNo - 1) * postsPerPage;
        ViewBag.CurrentPage = pageNo;
        ViewBag.pageCount = (int)Math.Ceiling((double)_context.Articles.Count() / postsPerPage);
        var recentArticles = _context.Articles.Include(x => x.Category).Include(x => x.User).OrderByDescending(x => x.Id).Skip(skipPage).Take(postsPerPage).ToList().Select(x => { x.Content = StripHTML(x.Content); return x; }).ToList();
        var trendVideos = _context.Articles.Where(x => x.CategoryId == 4).OrderByDescending(x => x.ViewCount).Take(3).ToList();
        var popular = _context.Articles.OrderByDescending(x => x.ViewCount).Take(3).ToList();


        HomeVM vm = new()
        {
            RecentArticles = recentArticles,
            TrendVideos = trendVideos,
            PopularPosts = popular
        };

        return View(vm);
    }



}
