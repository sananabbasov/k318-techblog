using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models;

public class Article : BaseEntity
{
    public string Title { get; set; }
    public int ViewCount { get; set; }
    public string PhotoUrl { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
