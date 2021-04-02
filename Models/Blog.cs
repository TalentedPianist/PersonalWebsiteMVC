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
          public Posts Posts { get; set; }
          public Comments Comments { get; set; }
          public List<Posts> AllPosts { get; set; } 
          public List<Comments> AllComments { get; set; }
     }

     public class Posts
     {
          [Key]
          public int PostID { get; set; }
          public int CategoryID { get; set; }
          public string PostContent { get; set ; }
          public string PostExcerpt { get; set; }
          public string PostTitle { get; set; }
          public string PostAuthor { get; set; }
          public string PostLocation { get; set; }
          public string PostIP { get; set; }
          public string PostActive { get; set; }
          public DateTime PostDate { get; set; }
          public int CommentCount { get; set; }
     }

     public class Comments
     {
          [Key]
          public int CommentID { get; set; }
          public int ParentID { get; set; }
          public string PhotoID { get; set; }
          [Display(Name="Name")]
          public string CommentAuthor { get; set; }
          [Display(Name ="Email")]
          public string CommentAuthorEmail { get; set; }
          [Display(Name ="Website")]
          public string CommentAuthorUrl { get; set; }
          public string CommentAuthorIP { get; set; }
          public DateTime CommentDate { get; set; }
          [Display(Name ="Comment")]
          public string CommentContent { get; set; }
          public string CommentType { get; set; }

     }
}
