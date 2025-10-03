using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PersonalWebsiteMVC.Areas.pCloud.Models
{
     public class PhotosModel
     {
          [JsonPropertyName("path")]
          public string Path { get; set; } = string.Empty;
          [JsonPropertyName("name")]
          public string Name { get; set; } = string.Empty;
          [JsonPropertyName("created")]
          public string Created { get; set; } = string.Empty;
          [JsonPropertyName("ismine")]
          public bool IsMine { get; set; }
          [JsonPropertyName("thumb")]
          public bool Thumb { get; set; }
          [JsonPropertyName("modified")]
          public string Modified { get; set; } = string.Empty;
          [JsonPropertyName("comments")]
          public int Comments { get; set; }
          [JsonPropertyName("id")]
          public string Id { get; set; } = string.Empty;
          [JsonPropertyName("isshared")]
          public bool IsShared { get; set; }
          [JsonPropertyName("icon")]
          public string Icon { get; set; } = string.Empty;
          [JsonPropertyName("isfolder")]
          public bool IsFolder { get; set; }
          [JsonPropertyName("parentfolderid")]
          public long ParentFolderId { get; set; }
          [JsonPropertyName("fileid")]
          public long? FileId { get; set; }
          [JsonPropertyName("height")]
          public int? Height { get; set; }
          [JsonPropertyName("width")]
          public int? Width { get; set; }
          [JsonPropertyName("hash")]
          public ulong? Hash { get; set; }
          [JsonPropertyName("size")]
          public long? Size { get; set; }
          [JsonPropertyName("category")]
          public int? Category { get; set; }
          [JsonPropertyName("contenttype")]
          public string? ContentType { get; set; }
          [JsonPropertyName("exifdatetime")]
          public ulong? ExifDateTime { get; set; }
     }
}
