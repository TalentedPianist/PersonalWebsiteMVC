using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Views.Shared.Components.Mobile
{
    [ViewComponent(Name = "Mobile")]
    public class Mobile : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
         
                return View("Default");
            
        }

        // https://www.c-sharpcorner.com/blogs/result-types-in-asp-net-core-mvc


    }
}
