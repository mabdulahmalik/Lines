using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SOL.Abstractions.Application;

namespace SOL.IdP;

class KeycloakAuthenticator(
    HttpClient httpClient,
    ICacheManager cacheManager,
    ILogger<KeycloakAuthenticator> logger)
{
    public async Task<string> GetAccessToken(string clientId, string clientSecret)
    {
        var cacheKey = $"access-token-{clientId}";
        var cachedValue = await cacheManager.Get(cacheKey);
        if (!String.IsNullOrWhiteSpace(cachedValue))
            return cachedValue;
        
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret)
        });
        
        var tokenEndpoint = $"realms/{KeycloakConst.RealmId}/protocol/openid-connect/token";
        var response = await httpClient.PostAsync(tokenEndpoint, content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(responseContent);
        
        var accessToken = json["access_token"].ToString();
        var expiresAt = DateTime.UtcNow.AddSeconds(json["expires_in"].Value<int>());

        logger.LogInformation("Refreshed Access Token for {ClientId} within {Realm}, expires @ {ExpiresAt:f}.",
            clientId, KeycloakConst.RealmId, expiresAt);

        await cacheManager.Set(cacheKey, accessToken, expiresAt);
        return accessToken;
    }
}