using System.Text.Json.Serialization;

namespace PersonalWebsiteMVC.Areas.pCloud.Models
{
     public class FolderModel
     {
          public string? Name { get; set; }
          public string? Created { get; set; }
          public bool? Thumb { get; set; }
          public string? Modified { get; set; }
          public bool? Isfolder { get; set; }
          public long? Fileid { get; set; }
          public string? Path { get; set; }
          public int? Category { get; set; }
          public string? Id { get; set; }
          public bool? Isshared { get; set; }
          public bool? Ismine { get; set; }
          public long? Size { get; set; }
          public long? Parentfolderid { get; set; }
          public string? Contenttype { get; set; }
          public string? Icon { get; set; }
     }
}
