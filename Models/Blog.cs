using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Models
{
     public class MixModel
     {
          public Posts? Posts { get; set; }
          public Comments Comments { get; set; } = new Comments();
          public List<Posts> AllPosts { get; set; } = default!;
          public List<Comments> AllComments { get; set; } = default!;
     }

     public class Posts
     {
          public enum Active
          {
               Yes,
               No
          }

          public enum Published
          {
               Yes,
               No
          }

          [Key]
          public int PostID { get; set; }
          public int? CategoryID { get; set; }
          public string? FeaturedImage { get; set; }
          public string? PostContent { get; set; }
          public string? PostExcerpt { get; set; }
          public string? PostTitle { get; set; }
          public string? PostAuthor { get; set; }
          public string? PostLocation { get; set; }
          public string? PostIP { get; set; }
          public Active PostActive { get; set; }
          public Published PostPublished { get; set; }
          public DateTime? PostDate { get; set; }
          public int? CommentCount { get; set; }
          [NotMapped]
          public IFormFile? FileUpload { get; set; }
          public string? Keywords { get; set; }
          public string? Description { get; set; }
     }

     public class Comments
     {
          [Key]
          public int CommentID { get; set; }
          public int? PostID { get; set; }

          public string? PhotoID { get; set; }
          [Display(Name = "Name")]
          [Required]
          public string? CommentAuthor { get; set; }
          [Display(Name = "Email")]
          [Required]
          public string? CommentAuthorEmail { get; set; }
          [Display(Name = "Website")]
          public string? CommentAuthorUrl { get; set; }
          public string? CommentAuthorIP { get; set; }
          public DateTime CommentDate { get; set; }
          [Display(Name = "Comment")]
          [Required]
          public string? CommentContent { get; set; }
          public string? CommentType { get; set; }

     }
}
