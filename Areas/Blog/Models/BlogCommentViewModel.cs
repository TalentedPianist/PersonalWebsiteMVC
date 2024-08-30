using PersonalWebsiteMVC.Models;
using X.PagedList;

namespace PersonalWebsiteMVC.Areas.Blog.Models
{
    public class BlogCommentViewModel
    {
        public Posts? Post { get; set; }
        public Comments Comment { get; set; } = new Comments();
        public List<Comments> Comments { get; set; } = new List<Comments>(); 
        public PagedList<Comments>? PagedComments { get; set; } 
    }
}
