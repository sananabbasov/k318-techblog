using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.ViewModels;

public class HomeVM
{
    public List<Article> RecentArticles { get; set; }
    public List<Article> TrendVideos { get; set; }
    public List<Article> PopularPosts { get; set; }
    public Advertisement SidebarAds { get; set; }
    public Advertisement InlineAds { get; set; }
}

