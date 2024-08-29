using PersonalWebsiteMVC.Models;
using X.PagedList;

namespace PersonalWebsiteMVC.Areas.Blog.Models
{
    public class BlogCommentViewModel
    {
        public Posts? Post { get; set; }
        public Comments? Comment { get; set; } 
        public List<Comments>? Comments { get; set; } 
        public PagedList<Comments>? PagedComments { get; set; } 
    }
}
