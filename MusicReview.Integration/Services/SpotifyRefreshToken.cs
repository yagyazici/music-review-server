using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using MusicReview.Domain.Models.Spotify.RefreshToken;
using MusicReview.Domain.Settings;
using Newtonsoft.Json;

namespace MusicReview.Integration.Services;

public class SpotifyRefreshToken
{
    private readonly IOptions<SpotifySettings> _spotifySettings;
    private readonly HttpClient _httpClient;

    public SpotifyRefreshToken(
        IOptions<SpotifySettings> spotifySettings,
        HttpClient httpClient)
    {
        _spotifySettings = spotifySettings;
        _httpClient = httpClient;
    }

    public async Task<RefreshToken> RefreshToken()
    {
        var body = new Dictionary<string, string>
        {
            {"grant_type", "refresh_token"},
            {"refresh_token", _spotifySettings.Value.RefreshToken}
        };
    
        HttpRequestMessage request = new HttpRequestMessage 
        {
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(body),
        };

        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _spotifySettings.Value.Base_64);

        var res = await _httpClient.SendAsync(request);
        var resString = await res.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<RefreshToken>(resString);
        return token;
    }
}