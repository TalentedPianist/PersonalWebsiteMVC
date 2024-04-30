using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using PersonalWebsiteBlazor.Models;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteBlazor.Components
{
    public partial class Mobile : ComponentBase
    {
        private NavigationManager _nav { get; set; }
        private JSRuntime JS { get; set; }
        public Mobile(NavigationManager nav, JSRuntime jS)
        {
            _nav = nav;
            JS = jS;
        }


        public string? Message { get; set; }

        [SupplyParameterFromForm]
        public Contact ContactModel { get; set; } = new Contact();

        public List<Posts>? BlogModel { get; set; } = new List<Posts>();

        public int? PostID { get; set; }

        public bool ShowContent { get; set; }



        public void GetQueryString()
        {
            var uri = _nav.ToAbsoluteUri(_nav.Uri);

            var queryStrings = QueryHelpers.ParseQuery(uri.Query);
            if (queryStrings.TryGetValue("id", out var id))
            {
                PostID = Convert.ToInt32(id);

            }
            StateHasChanged();
        }

     

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var uri = _nav.ToAbsoluteUri(_nav.Uri);
            if (firstRender)
            {
                await JS.InvokeVoidAsync("DetectScroll");

                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var id))
                {
                 
                }

                if (uri.Fragment.Contains("Home"))
                {
                    await JS.InvokeVoidAsync("SmoothScroll", "Home");
                }

                if (uri.Fragment.Contains("About"))
                {
                    await JS.InvokeVoidAsync("SmoothScroll", "About");
                }
                if (uri.Fragment.Contains("Blog"))
                {
                    if (PostID > 0)
                    {
                        await JS.InvokeVoidAsync("SmoothScroll", "SinglePost");
                       
                    }
                    else
                    {
                        await JS.InvokeVoidAsync("SmoothScroll", "Blog");
                 
                    }
                }

                if (uri.Fragment.Contains("Comments"))
                {
                    await JS.InvokeVoidAsync("SmoothScroll", "Comments");
                    
                }

                if (uri.Fragment.Contains("Contact"))
                {
                    await JS.InvokeVoidAsync("SmoothScroll", "Contact");
                }

               
     
                await JS.InvokeVoidAsync("LoadMore");


            }
        }

        private void Navigate(string id)
        {

            var query = new Dictionary<string, string>
{
            { "id", id }
        };
            PostID = Convert.ToInt32(id);
            // Argument of type dictionary cannot be used for parameter 'queryString' of type 'IDictionary<string, string>' due to differences in the nullability of reference  types just needs to be "query!".
            _nav.NavigateTo(QueryHelpers.AddQueryString(String.Empty, query!));

        }

        public async Task AboutMe()
        {
            await JS.InvokeVoidAsync("SmoothScroll", "About");
        }

        public async Task SubmitContactForm()
        {
            await Task.CompletedTask;
        }
    }
}
