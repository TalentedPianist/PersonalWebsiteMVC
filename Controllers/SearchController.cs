using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;
using ServiceStack;
using SharpCompress;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Exceptions;
using System.Collections.Generic;
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
               options.Fields = new[] { "id", "title", "link", "body" };
               options.Highlight = new HighlightingParameters
               {
                    Fields = new[] { "body" },
                    BeforeTerm = "<span class='highlight'>",
                    AfterTerm = "</span>",
                    Fragsize = 50,
               };
           
               

               var results = _solr.Query(new SolrQueryByField("body", Term), options);
               List<SearchModel> model = new List<SearchModel>();

               foreach (var result in results)
               {
                    var highlights = results.Highlights[result.Id];
                    highlights.Values.ForEach(h =>
                    {
                         h.ForEach(p =>
                         {
                              model.Add(new SearchModel { Id = result.Id, Title = result.Title, Link = result.Link, Body = p });
                         });
                    });
               }

               ViewBag.Term = Term;
               return PartialView("~/Views/Home/SearchResults.cshtml", model);

          }
     }
}
