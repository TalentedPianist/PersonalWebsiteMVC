using SolrNet.Attributes;

namespace PersonalWebsiteMVC.Models
{
     public class SearchModel
     {
          [SolrUniqueKey("id")]
          public string Id { get; set; } = default!;
          [SolrField("title_s")]
          public string Title { get; set; } = default!;
          [SolrField("link_s")]
          public string Link { get; set; } = default!;
          [SolrField("body_s")]
          public string Body { get; set; } = default!;

     }
}
