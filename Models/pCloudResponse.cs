namespace PersonalWebsiteMVC.Models
{

     public class PCloudResponse
     {
          public MetadataContainer? metadata { get; set; }
     }

     public class MetadataContainer
     {
          public string? icon { get; set; }
          public string? id { get; set; }
          public string? modified { get; set; }
          public string? path { get; set; }
          public bool? thumb { get; set; }
          public string? created { get; set; }
          public int? folderid { get; set; }
          public bool? isshared { get; set; }
          public bool? isfolder { get; set; }
          public bool? ismine { get; set; }
          public string? name { get; set; }

          public List<Contents>? Contents { get; set; }
     }

     public class Contents
     {
          public int? parentfolderid { get; set; }
          public string? id { get; set; }
          public string? modified { get; set; }
          public string? path { get; set; }
          public bool? thumb { get; set; }
          public string? created { get; set; }
          public string? folderid { get; set; }
          public bool? ismine { get; set; }
          public bool? isshared { get; set; }
          public bool? isfolder { get; set; }
          public string? name { get; set; }
          public string? icon { get; set; }
     }
}
