using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Models
{
	public class SolrModel
	{
		[SolrUniqueKey("id")]
		public string ID { get; set; }
		[SolrField("title")]
		public string Title { get; set; }
		[SolrField("url")]
		public string Url { get; set; }
		[SolrField("body")]
		public string Body { get; set; }
	}


}
