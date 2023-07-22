using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Models
{
     public class Categories
     {
          [Key]
          public int CategoryID { get; set; }
          public int PostID { get; set; }
          public int PostCount { get; set; }
        public string Category { get; set; } = default!;
        public string IP { get; set; } = default!;
          public DateTime CategoryDate { get; set; }
     }
}
