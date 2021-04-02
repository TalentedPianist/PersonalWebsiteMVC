using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PersonalWebsiteMVC.TagHelpers
{
     [HtmlTargetElement("HelloWorld")]
     public class HelloWorldTagHelper : TagHelper
     {
          public override void Process(TagHelperContext context, TagHelperOutput output)
          {
               output.TagName = "HelloWorld";
               output.TagMode = TagMode.StartTagAndEndTag;
               output.PreContent.SetHtmlContent("<h1>Hello World!</h1>");
          }
     }
}
