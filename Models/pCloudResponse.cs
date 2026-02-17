public class PCloudResponse
{
     public int result { get; set; }
     public pCloudMetadata? metadata { get; set; }
}

public class pCloudMetadata
{
     public string? icon { get; set; }
     public string? id { get; set; }
     public string? modified { get; set; }
     public string? path { get; set; }
     public bool? thumb { get; set; }
     public string? created { get; set; }
     public string? folderid { get; set; }
     public bool isshared { get; set; }
     public bool isfolder { get; set; }
     public bool ismine { get; set; }
     public string? name { get; set; }

     public List<ContentItem>? contents { get; set; }
}

public class ContentItem
{
     public string? parentfolderid { get; set; }
     public string? id { get; set; }
     public string? modified { get; set; }
     //public string path { get; set; }
     public bool thumb { get; set; }
     public string? created { get; set; }
     public string? folderid { get; set; }
     public bool ismine { get; set; }
     public bool isshared { get; set; }
     public bool isfolder { get; set; }
     public string? name { get; set; }
     public string? icon { get; set; }
     public string? path { get; set; }

     // Optional fields (only appear on files)
     public string? contenttype { get; set; }
     public string? hash { get; set; }
     public int? category { get; set; }
     public string? fileid { get; set; }
     public long? size { get; set; }
}