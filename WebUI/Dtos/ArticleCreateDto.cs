using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Dtos;

public class ArticleCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public List<int> TagIds { get; set; }


}
