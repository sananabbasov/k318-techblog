using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models;

public class Advertisement : BaseEntity
{
    public string PhotoUrl { get; set; }
    public string RedirectAddress { get; set; }
}
