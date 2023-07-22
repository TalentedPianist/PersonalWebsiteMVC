using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Posts> Posts { get; set; } = default!;
        public DbSet<Categories> Categories { get; set; } = default!;
        public DbSet<Comments> Comments { get; set; } = default!;
        public DbSet<Guestbook> Guestbook { get; set; } = default!;
    }
}