using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Models
{
     public class Gallery
     {
          [Key]
          public int GalleryID { get; set; }
          [Display(Name="Remote ID")]
          public string GalleryRemoteID { get; set; }
          [Display(Name = "Gallery Url")]
          public string GalleryUrl { get; set; }
          [Display(Name="Name")]
          public string GalleryName { get; set; }
          [Display(Name ="Description")]
          public string GalleryDescription { get; set; }
          [Display(Name ="Location")]
          public string GalleryLocation { get; set; }
          [Display(Name = "Date")]
          public DateTime? GalleryDate { get; set; }
          public string GalleryIP { get; set; }
     }

     public class Photos
     {
          [Key]
          public int PhotoID { get; set; }
          [Display(Name = "Remote ID")]
          public string PhotoRemoteID { get; set; }
          [Display(Name = "Select Photo")]
          public string GalleryRemoteID { get; set; }
          [Display(Name = "PhotoMediuUrl")]
          public string PhotoMediumUrl { get; set; }
          [Display(Name = "PhotoLargeUrl")]
          public string PhotoLargeUrl { get; set; }
          [Display(Name = "Name")]
          public string PhotoName { get; set; } 
          [Display(Name = "Description")]
          public string PhotoDescription { get; set; }
          [Display(Name = "Location")]
          public string PhotoLocation { get; set; }
          [Display(Name = "Date")]
          public DateTime PhotoDate { get; set; }
          public string PhotoIP { get; set; }
         
     }
}
