using CommonServiceLocator;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;
using System.Net;
using System.Text;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

// https://blexin.com/en/blog-en/how-to-integrate-elasticsearch-in-asp-net-core/
// https://www.c-sharpcorner.com/article/how-to-integrate-elasticsearch-in-asp-net-core/

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Authorize(Roles = "Admin")]
     [Area("Admin")]
     public class SearchController : Controller
     {
          public ElasticsearchClient _client { get; set; }

          public SearchController(ElasticsearchClient client)
          {
               _client = client;
          }

          public IActionResult Index()
          {
               return View();
          }

          public IActionResult Create()
          {

               return View(_client.);
          }

          [HttpPost]
          public IActionResult CreateIndex()
          {
               return View("Create");
          }

          private string ParseBody(string url)
          {
               var web = new HtmlWeb();
               var doc = web.Load(url);
               var nodes = doc.DocumentNode.SelectSingleNode("//body");
               var result = nodes.SelectNodes("//p");
               StringBuilder sb = new StringBuilder();
               foreach (var node in result)
               {
                    sb.Append(node.InnerText);
               }
               return sb.ToString();
          }

          private string ParseTitle(string url)
          {
               var web = new HtmlWeb();
               var doc = web.Load(url);
               var nodes = doc.DocumentNode.SelectSingleNode("//title");
               return nodes.InnerText;
          }

      
     }

}
