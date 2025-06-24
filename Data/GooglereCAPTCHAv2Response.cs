using System.Text.Json.Serialization;

namespace PersonalWebsiteMVC.Data;

public class GooglereCAPTCHAv2Response
{

    public bool Success { get; set; }
    [JsonPropertyName("challenge_ts")]
    public DateTimeOffset ChallengeTimestamp { get; set; }
    [JsonPropertyName("error-codes")]
    public string[] ErrorCodes { get; set; } = new string[0];
}