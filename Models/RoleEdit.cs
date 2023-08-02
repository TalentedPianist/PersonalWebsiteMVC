using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Models
{
     public class RoleEdit
     {
        public IdentityRole Role { get; set; } = default!;
        public IEnumerable<ApplicationUser> Members { get; set; } = default!;
        public IEnumerable<ApplicationUser> NonMembers { get; set; } = default!;
     }
}
