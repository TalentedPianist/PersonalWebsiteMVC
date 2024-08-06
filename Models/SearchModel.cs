namespace PersonalWebsiteMVC.Models
{
    public class SearchModel
    {
        public int? Id { get; set; } 
        public string? Title { get; set; } 
        public string? Excerpt { get; set; } 
        public string? Page { get; set; } 
        public string? Url { get; set; } 
        public string? Body { get; set; } 
        public DateTime? IndexDate { get; set; } 
    }
}
