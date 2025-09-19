using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Authentication;
using MyGraphSdk;
using PersonalWebsiteMVC.Helpers;


namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     public class OneDriveController : Controller
     {
       

          [Route("/Admin/OneDrive")]
          public IActionResult Index(Settings settings)
          {
               var scopes = new[] { "https://graph.microsoft.com/.default" };

               var clientId = "4cb4c3f8-b663-44bb-9f7c-ec3ee7e42edb";
               var tenantId = "6e07ac96-b10b-43e3-b9a4-37d7dfcddfab";
               var clientSecret = "2418Q~luZQBl7PxzwIq_Hnuz.nUlYtxScSc6daC-";

               var credential = new DeviceCodeCredential(options =>
               {
                    options.ClientId = "your-client-id";
                    options.TenantId = "your-tenant-id";
                    options.DeviceCodeCallback = info =>
                    {
                         Console.WriteLine(info.Message); // Prompts user to sign in
                         return Task.CompletedTask;
                    };
               });

               var authProvider = new AzureIdentityAuthenticationProvider(credential, scopes);
               var adapter = new BaseGraphRequestAdapter(authProvider);
               var graphClient = new GraphClient(adapter);

               var me = await graphClient.Me.GetAsync();


               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }

          [HttpPost]
          public IActionResult Login()
          {
               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }
         
     }
}
