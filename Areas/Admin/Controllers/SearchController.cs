using CommonServiceLocator;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Exceptions;
using SolrNet.Impl;
using System.Net;
using System.Text;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Authorize(Roles = "Admin")]
     [Area("Admin")]
     public class SearchController : Controller
     {
          private ISolrOperations<SearchModel> _solr;

          public SearchController(ISolrOperations<SearchModel> solr)
          {
               _solr = solr;
          }

          public IActionResult Index()
          {

               var q = _solr.Query(SolrQuery.All);
               StringBuilder sb = new StringBuilder();
             
               return View(q);
          }


          public IActionResult Create()
          {
               return View();
          }

          [HttpPost]
          public IActionResult CreateIndex([FromForm(Name="Url")]string Url, SearchModel model)
          {
               string body = ParseBody(Url);
               string title = ParseTitle(Url);
               model.Body = body;
               model.Title = title;
               _solr.Add(model);
               _solr.Commit();

               TempData["Message"] = model.Url;
               return View("~/Areas/Admin/Views/Search/Create.cshtml");
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

          
          public IActionResult Delete(string title)
          {
               StringBuilder sb = new StringBuilder();
               
               return View();
          }

          public IActionResult DeleteAll()
          {
      
               _solr.Delete(SolrQuery.All);
               _solr.Commit();
               return RedirectToAction("Index");
          }

          public IActionResult Details(string id)
          {

               var result = _solr.Query(new SolrQueryByField("id", id));
               SearchModel model = new SearchModel();
               model.Id = result[0].Id;
               model.Title = result[0].Title;
               model.Url = result[0].Url;
               model.Body = result[0].Body;
               return View(model);
          }

          public IActionResult Update(string id)
          {
               var result = _solr.Query(new SolrQueryByField("id", id));
               SearchModel model = new SearchModel();
               model.Id = result[0].Id;
               model.Title = result[0].Title;
               model.Url = result[0].Url;
               model.Body = result[0].Body;
               return View(model);
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/Admin/Search/UpdateSolr")]
          public IActionResult UpdateSolr(SearchModel model)
          {
               TempData["Message"] = model.Id;
               return View("Update", model);
          }
     }

}
