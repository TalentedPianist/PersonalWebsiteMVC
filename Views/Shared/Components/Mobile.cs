using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Views.Shared.ViewComponents
{
    [ViewComponent(Name="Mobile")]
    public class Mobile : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View());
        }
    }
}
