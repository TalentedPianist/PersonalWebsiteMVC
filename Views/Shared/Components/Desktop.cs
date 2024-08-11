using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Views.Shared.Components
{
    [ViewComponent(Name="Desktop")]
    public class Desktop : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View());
        }
    }
}
