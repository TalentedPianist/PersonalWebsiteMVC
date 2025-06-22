using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PersonalWebsiteMVC;

public class GoogleCaptchaResponseResult
{
    [JsonPropertyName("Success")]
    public bool Success { get; set; }
    [JsonPropertyName("error-codes")]
    public List<string> ErrorCodes { get; set; }
}