using Newtonsoft.Json;

namespace OnlineStore.Library.IdentityServer
{
    public class Token
    {
        [JsonProperty("token")] public string AccessToken { get; set; } // Changed from accessToken to accessToken

        [JsonProperty("token_type")] public string TokenType { get; set; } // Changed from tokenType to tokenType

        [JsonProperty("expires_in")] public int ExpiresIn { get; set; } // Changed from  expiresIn to expiresIn

        [JsonProperty("scope")] public string Scope { get; set; }
    }
}