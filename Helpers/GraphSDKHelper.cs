using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PersonalWebsiteMVC.Helpers;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Text;
using X.PagedList;

namespace PersonalWebsiteMVC.Helpers
{
     public interface IGraphSDKHelper
     {
          Task<GraphServiceClient> GraphClient();
     }
     public class GraphSDKHelper : IGraphSDKHelper
     {
          private IAuthHelper _Auth;
          private GraphServiceClient _graphClient;
          private IHttpContextAccessor _HttpContext;

          public GraphSDKHelper(IAuthHelper auth, IHttpContextAccessor http)
          {
               _Auth = auth;
               _HttpContext = http;
          }


          public async Task<GraphServiceClient> GraphClient()
          {
               _graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                    async requestMessage =>
                    {
                         var accessToken = await _Auth.AccessToken();
                         requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                         requestMessage.Headers.Add("User-Agent", "Google Chrome");
                         await Task.CompletedTask;
                    }));
               await Task.CompletedTask;
               return _graphClient;
          }

       

     }
}