using SolrNet.Attributes;

namespace PersonalWebsiteMVC.Models
{
    public class SearchModel
    {
          [SolrUniqueKey("Id")]
        public int? Id { get; set; } 
          [SolrField("Title")]
        public string? Title { get; set; }
          [SolrField("Excerpt")]
        public string? Excerpt { get; set; }
          [SolrField("Page")]
        public string? Page { get; set; }
          [SolrField("Url")]
        public string? Url { get; set; }
          [SolrField("Body")]
        public string? Body { get; set; }
          [SolrField("IndexDate")]
        public DateTime? IndexDate { get; set; } 
    }
}
