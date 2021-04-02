using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
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

          public DbSet<Posts> Posts { get; set; }
          public DbSet<Comments> Comments { get; set; }
          public DbSet<Guestbook> Guestbook { get; set; }
          public DbSet<Categories> Categories { get; set; }
          public DbSet<Gallery> Gallery { get; set; }
          public DbSet<Photos> Photos { get; set; }
     }
}
