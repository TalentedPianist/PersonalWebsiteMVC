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
using Elastic.Clients.Elasticsearch.QueryDsl;
using ServiceStack.Text;
using SharpCompress;
using NuGet.Protocol;
using ServiceStack;

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
               var docs = _client.Search<SearchModel>(s => s
                     .Query(s => s.MatchAll())).Documents.ToList();
             
               return View(docs);

          }



          public IActionResult Create()
          {

               return View();
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/Admin/Search/CreateIndex")]
          public async Task<IActionResult> CreateIndex()
          {
               var result = await _client.Indices.CreateAsync("blog");
               TempData["Message"] = result.DebugInformation;
               return View("~/Areas/Admin/Views/Search/Index.cshtml");
          }

          [HttpPost]
          public async Task<IActionResult> CreateDoc(SearchModel model)
          {
               Random rnd = new Random();
               var doc = new SearchModel
               {
                    Id = rnd.Next(1, 100),
                    Title = ParseTitle(model.Link!),
                    Link = model.Link,
                    Body = ParseBody(model.Link!)
               };
               var response = await _client.IndexAsync(doc, x => x.Index("blog"));


               TempData["Message"] = response.DebugInformation;

               return View("~/Areas/Admin/Views/Search/Create.cshtml");
          }

          [Microsoft.AspNetCore.Mvc.Route("/Admin/Search/Update/{id}")]
          public async Task<IActionResult> Update(int id)
          {
               var response = await _client.GetAsync<SearchModel>(id, x => x.Index("blog"));

               if (response.IsValidResponse)
                    return View(response.Source);
               else
                    return View();
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/Admin/Search/UpdateSearch")]
          public async Task<IActionResult> UpdateSearch([FromForm]SearchModel model)
          {
               var response = await _client.UpdateAsync<SearchModel, SearchModel>("blog", model.Id, u => u.Doc(model));
               if (response.IsValidResponse)
               {
                    TempData["Message"] = "Update document succeeded.";
               }
               return View("~/Areas/Admin/Views/Search/Update.cshtml", model);
          }

          public async Task<IActionResult> Details(int id)
          {
               var response = await _client.GetAsync<SearchModel>(id, x => x.Index("blog"));
               if (response.IsValidResponse)
                    return View("~/Areas/Admin/Views/Search/Details.cshtml", response.Source);
               else
                    return View("~/Areas/Admin/Views/Search/Details.cshtml");
          }

          public async Task<IActionResult> Delete(int id)
          {
               var response = await _client.GetAsync<SearchModel>(id, x => x.Index("blog"));
               var model = new SearchModel
               {
                   Title = response.Source!.Title,
                   Link = response.Source.Link,
                   Body = response.Source.Body
               };
               return View(model);
          }

          [HttpPost]
          public async Task<IActionResult> DeleteSearch([FromForm(Name="SearchID")]int Id)
          {
               var response = await _client.DeleteAsync("blog", Id);
               return RedirectToAction("Index");
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
