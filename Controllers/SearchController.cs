using Microsoft.AspNetCore.Mvc;
using SolrNet;

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


		// GET: /<controller>/
		public IActionResult Index()
		{
			return View();
		}
	}
}
