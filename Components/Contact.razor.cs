using Microsoft.AspNetCore.Components;
using Sitko.Blazor.CKEditor;
using Sitko.Blazor.CKEditor.Bundle;

namespace PersonalWebsiteMVC.Components
{
    public partial class Contact
    {
        public string? Message { get; set; }

        [SupplyParameterFromForm]
        public PersonalWebsiteMVC.Models.Contact Model { get; set; } = new PersonalWebsiteMVC.Models.Contact();

        public async Task HandleValidSubmit()
        {
            await Task.CompletedTask;
        }

        private CKEditorConfig? config;

        public Contact()
        {
            config = CKEditorBundleOptions.DefaultConfig.WithHtmlEditing();
            config.Toolbar = new CKEditorToolbar
            {
                
            };
            config.Placeholder = "Type your message here....";
        }

        public async Task HandleInvalidSubmit()
        {
            Message = "An error has occurred.";
            await Task.CompletedTask;
        }
    }
}
