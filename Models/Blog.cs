using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Models
{
     public class MixModel 
     {
        public Posts Posts { get; set; } = default!;
        public Comments Comments { get; set; } = default!;
        public List<Posts> AllPosts { get; set; } = default!;
        public List<Comments> AllComments { get; set; } = default!;
     }

     public class Posts
     {
          [Key]
          public int PostID { get; set; }
          public int CategoryID { get; set; }
        public string PostContent { get; set; } = default!;
        public string PostExcerpt { get; set; } = default!;
        public string PostTitle { get; set; } = default!;
        public string PostAuthor { get; set; } = default!;
        public string PostLocation { get; set; } = default!;
        public string PostIP { get; set; } = default!;
        public string PostActive { get; set; } = default!;
        public bool PostPublished { get; set; } = default!;
        public DateTime PostDate { get; set; } = default!;
          public int CommentCount { get; set; }
     }

     public class Comments
     {
          [Key]
          public int CommentID { get; set; }
          public int ParentID { get; set; }
        public string PhotoID { get; set; } = default!;
        [Display(Name = "Name")]
        public string CommentAuthor { get; set; } = default!;
        [Display(Name = "Email")]
        public string CommentAuthorEmail { get; set; } = default!;
        [Display(Name = "Website")]
        public string CommentAuthorUrl { get; set; } = default!;
        public string CommentAuthorIP { get; set; } = default!;
          public DateTime CommentDate { get; set; }
        [Display(Name = "Comment")]
        public string CommentContent { get; set; } = default!;
        public string CommentType { get; set; } = default!;

     }
}
