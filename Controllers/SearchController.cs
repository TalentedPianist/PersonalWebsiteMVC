using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;
using ServiceStack;
using SharpCompress;
using System.Text;

namespace PersonalWebsiteMVC.Controllers
{
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

          [Microsoft.AspNetCore.Mvc.Route("/Search")]
          [HttpPost]
          public async Task<IActionResult> GetText(string Term)
          {

               var response = await _client.SearchAsync<SearchModel>(s => s
                    .Indices("blog")
                    .Query(q => q
                         .Term(t => t
                              .Field(x => x.Body)
                              .Value(Term)
                              
                         )

                    )
                   .Highlight(h =>
                   {
                        h.PreTags("<b>");
                        h.PostTags("</b>");
                        h.RequireFieldMatch(true);
                        h.Fields(f =>
                        {
                            
                        });
                        h.FragmentSize(10);
                   })
                   
               );

               StringBuilder sb = new StringBuilder();

               foreach (var hit in response.Hits)
               {
                    if (hit.Highlight != null && hit.Highlight.TryGetValue("content", out var frags))
                         {
                         foreach (var frag in frags)
                         {
                              sb.Append(frag);
                         }
                    }
               }
               
              
               ViewBag.Result = sb.ToString();
               return PartialView("~/Views/Home/SearchResults.cshtml");

          }
     }
}
