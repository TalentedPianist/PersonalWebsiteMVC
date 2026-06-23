using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Models;
using RestSharp;

namespace PersonalWebsiteMVC.Api
{
     [Route("api/[controller]")]
     [ApiController]
     public class PCloudAuthentication : ControllerBase
     {
         
          // Correct useage of parameters - string? clientId - bypasses validation errors
          [HttpPost("/api/GetToken")]
          public IActionResult GetToken([FromQuery(Name="client_id")]string? clientId, [FromQuery(Name="client_secret")]string clientSecret, [FromQuery(Name="code")]string code)
          {
               var url = "https://eapi.pcloud.com/";
               var client = new RestClient(url);
               var request = new RestRequest("oauth2_token");
               request.AddParameter("client_id", clientId);
               request.AddParameter("client_secret", clientSecret);
               request.AddParameter("code", code);
               var response = client.Execute(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               var json = JsonConvert.DeserializeObject<pCloudToken>(response.Content!);
               var token = json?.access_token == null ? "NULL" : json.access_token;
               //Environment.SetEnvironmentVariable("PCloudToken", json!.access_token);
               return Ok(token);
          }

          [HttpGet("/pCloud/GetAccessToken")]
          public IActionResult GetAccessToken()
          {
               return Ok(Environment.GetEnvironmentVariable("PCloudToken"));
          }

          [HttpGet("/api/pCloud/ListFolder")]
          public IActionResult ListFolder([FromQuery(Name = "access_token")] string token, [FromQuery(Name = "folderid")] string folderid)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("listfolder");
               request.AddParameter("access_token", token);
               request.AddParameter("folderid", folderid);
               var response = client.Execute(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
               return Ok(response);
          }
     }
}

     
