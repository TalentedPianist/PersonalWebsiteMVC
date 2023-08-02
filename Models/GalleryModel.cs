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
        [Display(Name = "Remote ID")]
        public string GalleryRemoteID { get; set; } = default!;
        [Display(Name = "Gallery Url")]
        public string GalleryUrl { get; set; } = default!;
        [Display(Name = "Name")]
        public string GalleryName { get; set; } = default!;
        [Display(Name = "Description")]
        public string GalleryDescription { get; set; } = default!;
        [Display(Name = "Location")]
        public string GalleryLocation { get; set; } = default!;
          [Display(Name = "Date")]
          public DateTime? GalleryDate { get; set; }
        public string GalleryIP { get; set; } = default!;
     }

     public class Photos
     {
          [Key]
          public int PhotoID { get; set; }
        [Display(Name = "Remote ID")]
        public string PhotoRemoteID { get; set; } = default!;
        [Display(Name = "Select Photo")]
        public string GalleryRemoteID { get; set; } = default!;
        [Display(Name = "PhotoMediuUrl")]
        public string PhotoMediumUrl { get; set; } = default!;
        [Display(Name = "PhotoLargeUrl")]
        public string PhotoLargeUrl { get; set; } = default!;
        [Display(Name = "Name")]
        public string PhotoName { get; set; } = default!;
        [Display(Name = "Description")]
        public string PhotoDescription { get; set; } = default!;
        [Display(Name = "Location")]
        public string PhotoLocation { get; set; } = default!;
          [Display(Name = "Date")]
          public DateTime PhotoDate { get; set; }
        public string PhotoIP { get; set; } = default!;
         
     }
}
