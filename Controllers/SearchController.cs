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
                    .From(0)
                    .Size(10)
                    .Query(q => q
                         .Term(t => t
                              .Field(x => x.Body)
                              .Value(Term)

                         )

                    )
                    .Highlight(h => h
                         .PreTags("<span class='highlight'>")
                         .PostTags("</span>")
                         .Encoder(HighlighterEncoder.Html)
                         .Fields(new Dictionary<Field, HighlightField>
                         {
                              [new Field(nameof(SearchModel.Body))] = new HighlightField()
                         }
                    )

               ));

               StringBuilder sb = new StringBuilder();
               response.Hits.ForEach(h =>
               {
                    sb.Append(h.GetResponseStatus());
               });

               

               ViewBag.Result = sb.ToString();

               return PartialView("~/Views/Home/SearchResults.cshtml");

          }
     }
}
