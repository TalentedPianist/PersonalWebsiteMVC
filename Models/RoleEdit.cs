using Microsoft.AspNetCore.Identity;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Models
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; } = default!;
        public IEnumerable<ApplicationUser> Members { get; set; } = default!;
        public IEnumerable<ApplicationUser> NonMembers { get; set; } = default!;
    }
}
