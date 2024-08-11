using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Views.Shared.Components
{
    [ViewComponent(Name="Tablet")]
    public class Tablet : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View());
        }
    }
}
