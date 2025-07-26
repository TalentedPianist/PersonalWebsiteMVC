
using Newtonsoft.Json;

public class CaptchaResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }
}