using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Models
{
     public class RoleModification
     {
        [Required]
        public string RoleName { get; set; } = default!;
        public string RoleId { get; set; } = default!;
        public string[] AddIds { get; set; } = default!;
        public string[] DeleteIds { get; set; } = default!;
     }
}
