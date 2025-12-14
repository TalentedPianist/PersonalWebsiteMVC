using CommonServiceLocator;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using PersonalWebsiteMVC.Models;
using SharpCompress;
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
          private readonly ISolrOperations<SearchModel> _solr;
          private readonly ISolrCoreAdmin _solrCoreAdmin;

          public SearchController(ISolrOperations<SearchModel> solr, ISolrCoreAdmin solrCoreAdmin)
          {
               _solr = solr;
               _solrCoreAdmin = solrCoreAdmin;
          }

          public IActionResult Index()
          {
               StringBuilder sb = new StringBuilder();
               var q = _solr.Query(SolrQuery.All);
               return View(q);
          }

          public IActionResult Create()
          {
               return View();
          }

          public IActionResult Update(string id)
          {
               var result = _solr.Query(SolrQuery.All,
                    new QueryOptions
                    {
                         RequestHandler = new RequestHandlerParameters("/get"),
                         ExtraParams = new Dictionary<string, string>
                         {
                              {"ids", id }
                         }
                    });
               
               return View(result.FirstOrDefault());
          }

          [HttpPost]

          public IActionResult UpdateDoc(SearchModel model)
          {
               _solr.Add(model);
               _solr.Commit();
               return RedirectToAction("Index");
          }

          [HttpPost]
          [Route("/Admin/Search/CreateDoc")]
          public IActionResult CreateDoc(SearchModel model)
          {
           
                    model.Title = ParseTitle(model.Link!);
                    model.Body = ParseBody(model.Link!);
                    _solr.Add(model);
                    _solr.Commit();
               TempData["Message"] = model.Title;
               
               return View("Create");
          }

          public IActionResult Details(string id)
          {
               var result = _solr.Query(SolrQuery.All,
                    new QueryOptions
                    {
                         RequestHandler = new RequestHandlerParameters("/get"),
                         ExtraParams = new Dictionary<string, string>
                         {
                              {"ids", id }
                         }
                    });
               return View(result.FirstOrDefault());
          }

          public IActionResult Delete(string id)
          {
               var model = _solr.Query(SolrQuery.All,
                    new QueryOptions
                    {
                         RequestHandler = new RequestHandlerParameters("/get"),
                         ExtraParams = new Dictionary<string, string>
                         {
                              { "ids", id }
                         }
                    });
               _solr.Delete(model);
               _solr.Commit();
               return RedirectToAction("Index");
          }

          [HttpPost]
          [Route("/Admin/Search/DeleteAll")]
          public IActionResult DeleteAllDocs()
          {
               _solr.Delete(SolrQuery.All);
               _solr.Commit();
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
