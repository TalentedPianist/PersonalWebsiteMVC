using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Views.Shared.ViewComponents
{
    [ViewComponent(Name="Mobile")]
    public class Mobile : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View());
        }

        // https://www.c-sharpcorner.com/blogs/result-types-in-asp-net-core-mvc
        public PartialViewResult BlogPartial()
        {
            /* return new PartialViewResult()
             {
                 ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                 {
                     Model = new Posts()
                 }

             };*/

     
        }
    }
}
