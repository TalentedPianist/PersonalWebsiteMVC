namespace PersonalWebsiteMVC.Models
{
     public class Group
     {
          public string? id { get; set; }
          public string? displayName { get; set; }
     }

     public class Groups
     {
          public int? itemsPerPage { get; set; }
          public int? startIndex { get; set; }
          public int? totalResults { get; set; }
          public List<string>? resources { get; set; }

     }
}
