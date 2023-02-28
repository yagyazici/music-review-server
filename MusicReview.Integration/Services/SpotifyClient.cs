using System.Net.Http.Headers;
using MusicReview.Domain.Models.Spotify.Common;
using MusicReview.Domain.Models.Spotify.CurrentSong;
using MusicReview.Domain.Models.Spotify.GetArtistAlbums;
using MusicReview.Domain.Models.Spotify.Search;
using MusicReview.Domain.SpotifyServices;
using Newtonsoft.Json;

namespace MusicReview.Integration.Services;
public class SpotifyClient : ISpotifyClient
{
    private readonly HttpClient _httpClient;
    private SpotifyRefreshToken _refreshToken;

    public SpotifyClient(
        HttpClient httpClient,
        SpotifyRefreshToken refreshToken)
    {
        _httpClient = httpClient;
        _refreshToken = refreshToken;
    }

    public async Task<SpotifyCurrentSong> CurrentSong(string accessToken)
    {   
        var url = new UriBuilder(_httpClient.BaseAddress.AbsoluteUri + "/me/player/currently-playing");
        url.Query = "market=gb";

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = url.Uri,
            Method = HttpMethod.Get
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var res = await _httpClient.SendAsync(request);
        var resString = await res.Content.ReadAsStringAsync();
        var currentSong = JsonConvert.DeserializeObject<SpotifyCurrentSong>(resString);
        return currentSong;
    }

    public async Task<List<SearchItem>> SearchAlbum(string search, string accessToken)
    {
        var url = new UriBuilder(_httpClient.BaseAddress.AbsoluteUri + "/search");

        url.Query = $"q={search}&type=album&market=gb";

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = url.Uri,
            Method = HttpMethod.Get,
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var res = await _httpClient.SendAsync(request);
        var resString = await res.Content.ReadAsStringAsync();
        var searchs = JsonConvert.DeserializeObject<SpotifySearch>(resString);
        return searchs.albums.items;
    }

    public async Task<SpotifyAlbum> GetAlbum(string albumId, string accessToken)
    {
        var url = new UriBuilder(_httpClient.BaseAddress.AbsoluteUri + $"/albums/{albumId}");
        
        url.Query = "market=gb";

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = url.Uri,
            Method = HttpMethod.Get,
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var res = await _httpClient.SendAsync(request);
        var resString = await res.Content.ReadAsStringAsync();
        var album = JsonConvert.DeserializeObject<SpotifyAlbum>(resString);
        return album;
    }

    public async Task<SpotifyArtist> GetArtist(string artistId, string accessToken)
    {
        var url = new UriBuilder(_httpClient.BaseAddress.AbsoluteUri + $"/artists/{artistId}");
        
        url.Query = "market=gb";

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = url.Uri,
            Method = HttpMethod.Get,
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var res = await _httpClient.SendAsync(request);
        var resString = await res.Content.ReadAsStringAsync();
        var artist = JsonConvert.DeserializeObject<SpotifyArtist>(resString);
        return artist;
    }

    public async Task<List<GetArtistAlbumsItem>> GetArtistAlbums(string artistId, string type, string accessToken)
    {
        var url = new UriBuilder(_httpClient.BaseAddress.AbsoluteUri + $"/artists/{artistId}/albums");
        
        url.Query = $"market=gb&include_groups={type}";

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = url.Uri,
            Method = HttpMethod.Get,
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var res = await _httpClient.SendAsync(request);
        var resString = await res.Content.ReadAsStringAsync();
        var albums = JsonConvert.DeserializeObject<GetArtistAlbums>(resString);
        return albums.items;
    }
}