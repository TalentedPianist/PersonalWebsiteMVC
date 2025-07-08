using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PersonalWebsiteMVC.Services;

public class GoogleCaptchaCheckResponseResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("error-codes")]
    public List<string> ErrorCodes { get; set; } = new();
}