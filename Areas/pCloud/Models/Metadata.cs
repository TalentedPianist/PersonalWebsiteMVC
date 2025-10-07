using PersonalWebsiteMVC.Areas.pCloud.Models;
public class Metadata
{
    public string? Path { get; set; }
    public string? Name { get; set; }
    public string? Created { get; set; }
    public bool? Ismine { get; set; }
    public bool? Thumb { get; set; }
    public string? Modified { get; set; }
    public int? Comments { get; set; }
    public string? Id { get; set; }
    public bool? Isshared { get; set; }
    public string? Icon { get; set; }
    public bool? Isfolder { get; set; }
    public long? Parentfolderid { get; set; }
    public long? Folderid { get; set; }

    public List<FolderModel>? Contents { get; set; } 
    
}