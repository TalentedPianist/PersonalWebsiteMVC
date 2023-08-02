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
		public string ID { get; set; } = default!;
		[SolrField("title")]
		public string Title { get; set; } = default!;
		[SolrField("url")]
		public string Url { get; set; } = default!;
		[SolrField("body")]
		public string Body { get; set; } = default!;
	}


}
