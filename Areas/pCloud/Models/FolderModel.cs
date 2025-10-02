namespace PersonalWebsiteMVC.Areas.pCloud.Models
{
     public class FolderModel
     {
          public string? icon { get; set; }
          public string? id { get; set; }
          public string? modified { get; set; }
          public string? path { get; set; }
          public bool? thumb { get; set; }
          public DateTime? created { get; set; }
          public int folderid { get; set; }
          public bool isshared { get; set; }
          public bool isfolder { get; set; }
          public bool ismine { get; set; }
          public string? name { get; set; }
     }
}
