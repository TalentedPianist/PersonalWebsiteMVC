using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PersonalWebsiteMVC.Helpers
{
     // https://riptutorial.com/asp-net-core/example/22277/model-validation-with-custom-attribute
     public class ProfanityValidatorAttribute : ValidationAttribute
     {
          protected override ValidationResult IsValid(object value, ValidationContext validationContext)
          {
               string[] lines = System.IO.File.ReadAllLines("Helpers\\badwords.txt");
            
               foreach (var line in lines)
               {
                    
               }
               return ValidationResult.Success;
          }

         
     }
}
