using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PersonalWebsiteMVC.TagHelpers
{
     [HtmlTargetElement("PicEditForm-firstChild")]
     public class PicEditFormFirstChild : TagHelper
     {
          [HtmlAttributeName("gallery-id")]
          public ModelExpression GalleryID { get; set; }
          [HtmlAttributeName("pic-name")]
          public ModelExpression PicName { get; set; }

          

          public override void Process(TagHelperContext context, TagHelperOutput output)
          {
               
          }
     }
}
