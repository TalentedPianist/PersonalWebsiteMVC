using Microsoft.AspNetCore.Mvc;

[ViewComponentAttribute]
public class Blog : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync() 
    {
        return View();
    }
}