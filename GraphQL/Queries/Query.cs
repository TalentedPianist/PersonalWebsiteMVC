
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

public class Query
{
    public async Task<List<Posts>> GetPosts([Service] ApplicationDbContext context) =>
        await context.Posts.ToListAsync();
    
}