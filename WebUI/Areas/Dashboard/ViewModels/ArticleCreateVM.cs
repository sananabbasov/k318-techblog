using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Areas.Dashboard.ViewModels;

public class ArticleCreateVM
{
    public List<Category> Categories { get; set; }
    public List<Tag> Tags { get; set; }
}
