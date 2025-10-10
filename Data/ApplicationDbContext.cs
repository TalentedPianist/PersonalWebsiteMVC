using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteBlazor.Models;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PersonalWebsiteMVC.Models.Posts> Posts { get; set; } = default!;
        public DbSet<Categories> Categories { get; set; } = default!;
        public DbSet<PersonalWebsiteMVC.Models.Comments> Comments { get; set; } = default!;
        public DbSet<Guestbook> Guestbook { get; set; } = default!;
        public DbSet<Album> Albums { get; set; } = default!;
        public DbSet<Photos> Photos { get; set; } = default!;
          public DbSet<Portfolio> Portfolio { get; set; } = default!;
    }

    
}