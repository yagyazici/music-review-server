using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MusicReview.Domain.Settings;

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

    public async Task<string> RefreshToken()
    {
        var query = "https://accounts.spotify.com/api/token";

        var values = new Dictionary<string, string>
        {
            {"Authorization", $"Basic {_spotifySettings.Value.Base_64}"},
            {"Content-Type", "application/x-www-form-urlencoded"}
        };

        var body = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", $"{_spotifySettings.Value.RefreshToken}")
        };

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, query);

        request.RequestUri = new Uri(query);
        request.Headers.Add("Authorization", $"Basic {_spotifySettings.Value.Base_64}");
        request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        
        // _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        var res = await _httpClient.SendAsync(request);
        Console.WriteLine(res);
        return await res.Content.ReadAsStringAsync();
    }
}