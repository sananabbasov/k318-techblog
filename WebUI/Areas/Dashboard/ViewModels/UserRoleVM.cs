using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebUI.Models;

namespace WebUI.Areas.Dashboard.ViewModels;

public class UserRoleVM
{
    public User User { get; set; }
    public List<IdentityRole> Roles { get; set; }
}
