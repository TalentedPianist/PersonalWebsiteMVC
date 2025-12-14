using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;
using ServiceStack;
using SharpCompress;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Exceptions;
using System.Text;

namespace PersonalWebsiteMVC.Controllers
{
     public class SearchController : Controller
     {
          public ISolrOperations<SearchModel> _solr { get; set; }

          public SearchController(ISolrOperations<SearchModel> solr)
          {
               _solr = solr;
          }

          public IActionResult Index()
          {
               return View();
          }

          [Microsoft.AspNetCore.Mvc.Route("/Search")]
          [HttpPost]
          public IActionResult GetText(string Term)
          {
               StringBuilder sb = new StringBuilder();

               var options = new QueryOptions();
               options.Rows = 25;
               options.ExtraParams = new Dictionary<string, string>
               {
                    { "qf", $"{Term}" }
               };

               var model = _solr.Query(Term, options);
               ViewBag.Result = Term;

               return PartialView("~/Views/Home/SearchResults.cshtml", model);

          }
     }
}
