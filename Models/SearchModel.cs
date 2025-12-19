using SolrNet.Attributes;

namespace PersonalWebsiteMVC.Models
{
     public class SearchModel
     {
          [SolrUniqueKey("id")]
          public string Id { get; set; } = default!;
          [SolrField("title")]
          public string Title { get; set; } = default!;
          [SolrField("link")]
          public string Link { get; set; } = default!;
          [SolrField("body")]
          public string Body { get; set; } = default!;

     }
}
