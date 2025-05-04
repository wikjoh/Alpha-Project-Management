using System.Text.Json.Serialization;

namespace Business.Dtos.API;

public class CookieConsent
{
    [JsonPropertyName("essential")]
    public bool Essential { get; set; }

    [JsonPropertyName("functional")]
    public bool Functional { get; set; }
}
