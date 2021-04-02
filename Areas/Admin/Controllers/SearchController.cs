using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Mapping;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SearchController : Controller
	{
		private ISolrOperations<SolrModel> solr;
		private ISolrConnection _solrConnection;

		public SearchController(ISolrOperations<SolrModel> solr, ISolrConnection solrConnection)
		{
			this.solr = solr;
			_solrConnection = solrConnection;
		}

		// GET: /<controller>/
		public IActionResult Index()
		{
			var options = new QueryOptions();
			options.ExtraParams = new KeyValuePair<string, string>[] {
				//new KeyValuePair<string, string>("wt", "xml");
			};
			var model = solr.Query(new SolrQuery("*:*"), options);
			return View(model);
		}

		public IActionResult Create()
		{
			return View();
		}

		public IActionResult Update()
		{
			string id = HttpContext.Request.Query["q"];
			QueryOptions options = new QueryOptions();
			// https://github.com/SolrNet/SolrNet/blob/master/Documentation/CRUD.md
			options.RequestHandler = new RequestHandlerParameters("/get");
			options.ExtraParams = new Dictionary<string, string>
			{
				{"ids", id}
			};
			SolrQueryResults<SolrModel> results = solr.Query(new SolrQuery("*:*"), options);
			var model = new PersonalWebsiteMVC.Models.SolrModel();
			model.ID = results.Select(r => r.ID).FirstOrDefault();
			model.Title = results.Select(r => r.Title).FirstOrDefault();
			model.Url = results.Select(r => r.Url).FirstOrDefault();
			model.Body = results.Select(r => r.Body).FirstOrDefault();
			return View(model);
		}

		[HttpPost]
		public IActionResult Update(PersonalWebsiteMVC.Models.SolrModel model)
		{
			var s = new PersonalWebsiteMVC.Models.SolrModel();
			s.ID = model.ID;
			s.Title = model.Title;
			s.Url = model.Url;
			s.Body = model.Body;
			solr.Add(s);
			solr.Commit();
			return RedirectToAction("Index");
		}

		
		public IActionResult Details()
		{
			var options = new QueryOptions();
			options.ExtraParams = new KeyValuePair<string, string>[] {
				new KeyValuePair<string, string>("ids", HttpContext.Request.Query["q"])
			};
			SolrQueryResults<SolrModel> results = solr.Query("*:*", options);
			var model = new PersonalWebsiteMVC.Models.SolrModel();
			model.ID = results.Select(r => r.ID).FirstOrDefault();
			model.Title = results.Select(r => r.Title).FirstOrDefault();
			model.Url = results.Select(r => r.Url).FirstOrDefault();
			model.Body = results.Select(r => r.Body).FirstOrDefault();
			return View(model);
		}

		public IActionResult Delete()
		{
			var options = new QueryOptions();
			options.ExtraParams = new KeyValuePair<string, string>[] {
				//new KeyValuePair<string, string>("wt", "xml");
			};
			string id = HttpContext.Request.Query["q"];
			SolrQueryByField results = new SolrQueryByField("ID", id);
			solr.Delete(results.FieldValue);
			solr.Commit();
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Solr(PersonalWebsiteMVC.Models.SolrModel model)
		{
			var s = new PersonalWebsiteMVC.Models.SolrModel();
			s.Title = model.Title;
			s.Url = model.Url;
			s.Body = GetData(model.Url);
			solr.Add(s);
			solr.Commit();
			return RedirectToAction("Index");
		}
		
		string GetData(string url)
		{
			var html = new HtmlDocument();
			html.LoadHtml(new WebClient().DownloadString(url));
			var root = html.DocumentNode;
			var main = root.SelectSingleNode("//main");
			return main.InnerText;
		}
	}
}
