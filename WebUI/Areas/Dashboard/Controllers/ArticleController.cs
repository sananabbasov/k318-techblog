using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebUI.Areas.Dashboard.ViewModels;
using WebUI.Data;
using WebUI.Dtos;
using WebUI.Models;

namespace WebUI.Areas.Dashboard.Controllers;


[Area("Dashboard")]
[Authorize(Roles ="Admin, Moderator")]
public class ArticleController : Controller
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IWebHostEnvironment _env;
    public ArticleController(AppDbContext context, IHttpContextAccessor httpContext, IWebHostEnvironment env)
    {
        _context = context;
        _httpContext = httpContext;
        _env = env;
    }

    public IActionResult Index()
    {
        var articles = _context.Articles.Include(x => x.Category).Include(x => x.ArticleTags).ThenInclude(x => x.Tag).ToList();
        return View(articles);
    }

    public IActionResult Create()
    {
        var categories = _context.Categories.ToList();
        var tags = _context.Tags.ToList();

        ArticleCreateVM vm = new()
        {
            Tags = tags,
            Categories = categories
        };
        return View(vm);
    }


    [HttpPost]
    public IActionResult Create(ArticleCreateDto articleCreate, IFormFile photo)
    {
        var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var path = "/uploads/" + Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
        using (var stream = new FileStream(_env.WebRootPath + path, FileMode.Create))
        {
            photo.CopyTo(stream);
        }

        List<ArticleTag> tags = new();

        Article newArticle = new()
        {
            Title = articleCreate.Title,
            Content = articleCreate.Description,
            CategoryId = articleCreate.CategoryId,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            UserId = userId,
            PhotoUrl = path,
        };
        _context.Articles.Add(newArticle);
        _context.SaveChanges();


        List<ArticleTag> articleTags = new();

        for (int i = 0; i < articleCreate.TagIds.Count(); i++)
        {
            articleTags.Add(
                new ArticleTag
                {
                    ArticleId = newArticle.Id,
                    TagId = articleCreate.TagIds[i]
                }
                 );
        }

        _context.ArticleTags.AddRange(articleTags);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }


    public IActionResult Edit(int id)
    {
        var article = _context.Articles.Include(x => x.ArticleTags).FirstOrDefault(x => x.Id == id);
        var categories = _context.Categories.ToList();
        var tags = _context.Tags.ToList();

        ArticleEditVM vm = new()
        {
            FindedArticle = article,
            Categories = categories,
            Tags = tags
        };
        return View(vm);
    }


    [HttpPost]
    public IActionResult Edit(Article article, List<int> TagIds, string? photoUrl, IFormFile file)
    {

        if (file != null)
        {
            var path = "/uploads/" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            using (var stream = new FileStream(_env.WebRootPath + path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            article.PhotoUrl = path;
        }


        var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        article.UserId = userId;
        _context.Articles.Update(article);
        var a = _context.Articles.Include(x => x.ArticleTags).FirstOrDefault(x => x.Id == article.Id);
        _context.ArticleTags.RemoveRange(a.ArticleTags);
        List<ArticleTag> newTags = new();
        for (int i = 0; i < TagIds.Count(); i++)
        {
            newTags.Add(new ArticleTag
            {
                TagId = TagIds[i],
                ArticleId = article.Id
            });
        }
        a.ArticleTags.AddRange(newTags);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

}
