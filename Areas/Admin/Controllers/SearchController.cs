using CommonServiceLocator;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;
using SolrNet;
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
          private readonly ISolrOperations<SearchModel> _solr;
          private readonly ISolrCoreAdmin _solrCoreAdmin;

          public SearchController(ISolrOperations<SearchModel> solr, ISolrCoreAdmin solrCoreAdmin)
          {
               _solr = solr;
               _solrCoreAdmin = solrCoreAdmin;
          }

          public IActionResult Index()
          {
               var model = _solr.Query(SolrQuery.All);
               return View(model);

          }

          public IActionResult Create()
          {
               return View();
          }

          [HttpPost]
          [Route("/Admin/Search/CreateDoc")]
          public IActionResult CreateDoc(SearchModel model)
          {
               _solr.Add(model);
               _solr.Commit();
               return View("~/Areas/Admin/Views/Search/Create.cshtml", model);
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
