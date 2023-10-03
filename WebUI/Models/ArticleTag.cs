using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models;

public class ArticleTag : BaseEntity
{
    public int ArticleId { get; set; }
    public Article Article { get; set; }
    public int TagId { get; set; }
    public Tag Tag { get; set; }
}
