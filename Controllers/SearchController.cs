using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SolrNet;
using SolrNet.Commands.Parameters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalWebsiteMVC.Controllers
{
	public class SearchController : Controller
	{
		private ISolrOperations<PersonalWebsiteMVC.Models.SolrModel> _solr;

		public SearchController(ISolrOperations<PersonalWebsiteMVC.Models.SolrModel> solr)
		{
			_solr = solr;
		}

		[HttpPost]
		public IActionResult Search(string Search)
		{
			//return View("~/Views/Search/Index.cshtml");
			return RedirectToAction("Index", "Search", new { q = Search });
		}


		// GET: /<controller>/
		public IActionResult Index()
		{
			var queryOptions = new QueryOptions
			{

				Highlight = new HighlightingParameters
				{
					Fragsize = 150,
					Fields = new[] { "body" },
					BeforeTerm = "<span style='background-color: yellow; font-weight: bold;'>",
					AfterTerm = "</span>",

				}


			};
			var results = _solr.Query(new SolrQueryByField("body", HttpContext.Request.Query["q"]), queryOptions);
			var highlights = results.Highlights;

			var resultCount = results.Highlights.Count;
			if (resultCount == 0)
			{
				TempData["Message"] = "<h2>Search</h2><p>No results have been found for your search.  Please try something else.</p>";
				return View();
			}
			else
			{
				var searchResults = new List<PersonalWebsiteMVC.Models.SolrModel>();
				for (int i = 0; i < resultCount; i++)
				{
					var highlight = new PersonalWebsiteMVC.Models.SolrModel()
					{
						ID = results[i].ID,
						Title = results[i].Title,
						Url = results[i].Url,
						//Body = results[i].Body
					};

					// highlights are a separate array, and can be an aarray of hits...
					foreach (var h in highlights[results[i].ID])
					{
						highlight.Body += string.Join(",", h.Value.ToArray());
					}
					searchResults.Add(highlight);
				}
				return View(searchResults);
			}

			
		}
	}
}
