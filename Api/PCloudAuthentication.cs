using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using RestSharp;

namespace PersonalWebsiteMVC.Api
{
     [Route("api/[controller]")]
     [ApiController]
     public class PCloudAuthentication : ControllerBase
     {

          public ApplicationDbContext _db { get; set; }
          public IHttpContextAccessor _http { get; set; }

          public PCloudAuthentication(ApplicationDbContext db, IHttpContextAccessor http)
          {
               _db = db;
               _http = http;
          }

         
          // Correct useage of parameters - string? clientId - bypasses validation errors
          [HttpPost("/api/GetToken")]
          public IActionResult GetToken([FromQuery(Name="client_id")]string? clientId, [FromQuery(Name="secret")]string? clientSecret, [FromQuery(Name="code")]string? code)
          {
               var url = "https://eapi.pcloud.com/";
               var client = new RestClient(url);
               var request = new RestRequest("oauth2_token");
               request.AddParameter("client_id", "GJR8uDME26u");
               request.AddParameter("client_secret", "U83OQca6ABpaiDtaBsStUbgKRiAk");
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
               return Ok(token);
          }

          [HttpPost("/api/StoreToken")]
          public IActionResult StoreToken([FromQuery(Name="token")]string? token)
          {
               Environment.SetEnvironmentVariable("pCloudToken", token);
               foreach (var d in Environment.GetEnvironmentVariables())
               {
                    Console.WriteLine(d);
               }
               return Ok(token);
          }

          [HttpGet("/api/pCloudToken")]
          public IActionResult GetPCloudToken()
          {
               var token = Environment.GetEnvironmentVariable("pCloudToken");
               Console.WriteLine(token);
               return Ok(token);
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

          [HttpGet("/api/pCloud/ListAlbum")]
          public IActionResult ListAlbum([FromQuery(Name="token")]string token, [FromQuery(Name="id")]string id)
          {
               var client = new RestClient("https://eapi.pcloud.com");
               var request = new RestRequest("listfolder");
               request.AddParameter("folderid", id);
               request.AddParameter("code", token);
               request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
               request.AddParameter("password", "Inkyfrog1");
               var result = client.Execute(request);
               return Ok(result);
          }
     }
}

     
