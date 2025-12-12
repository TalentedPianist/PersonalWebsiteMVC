using SolrNet.Attributes;

namespace PersonalWebsiteMVC.Models
{
     public class SearchModel
     {
          [SolrUniqueKey("id")]
          public string? Id { get; set; }
          [SolrField("title_s")]
          public string? Title { get; set; }
          [SolrField("link_s")]
          public string? Link { get; set; }
          [SolrField("body_s")]
          public string? Body { get; set; }

     }
}
