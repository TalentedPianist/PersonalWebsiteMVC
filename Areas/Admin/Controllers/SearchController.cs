using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
using PersonalWebsiteMVC.Models;
using Microsoft.AspNetCore.Authorization;
using SolrNet;
using CommonServiceLocator;
using SolrNet.Commands.Parameters;
using System.Net;
using SolrNet.Exceptions;
using SolrNet.Impl;
using System.Text;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Authorize(Roles = "Admin")]
     [Area("Admin")]
     public class SearchController : Controller
     {

          public IActionResult Index()
          {
               //Startup.Init<SearchModel>("http://localhost:8983/solr/SearchModel");
               var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SearchModel>>();

               var result = solr.Query(SolrQuery.All, new QueryOptions
               {
                    ExtraParams = new Dictionary<string, string> {
                         {"timeAllowed", "100" },
                    }
               });
               StringBuilder sb = new StringBuilder();
               foreach (var item in result)
               {
                    sb.Append(item.Id);
               }
               
               return View(result);
          }


          public IActionResult Create()
          {
               return View();
          }
     }

}
