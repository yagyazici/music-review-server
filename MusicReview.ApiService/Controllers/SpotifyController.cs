using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicReview.Domain.Models.Spotify.Common;
using MusicReview.Domain.Models.Spotify.CurrentSong;
using MusicReview.Domain.Models.Spotify.GetArtistAlbums;
using MusicReview.Domain.Models.Spotify.RefreshToken;
using MusicReview.Domain.Models.Spotify.Search;
using MusicReview.Domain.SpotifyServices;
using MusicReview.Integration.Services;

namespace MusicReview.ApiService.Controllers;

[ApiController, Authorize]
[Route("[controller]/[action]")]
public class SpotifyController : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient;
    private readonly SpotifyRefreshToken _refreshToken;

    public SpotifyController(
        ISpotifyClient spotifyClient,
        SpotifyRefreshToken refreshToken)
    {
        _spotifyClient = spotifyClient;
        _refreshToken = refreshToken;
    }

    [HttpGet]
    public async Task<RefreshToken> RefreshToken() => await _refreshToken.RefreshToken();

    [HttpGet]
    public async Task<SpotifyCurrentSong> CurrentSong(string accessToken) => 
        await _spotifyClient.CurrentSong(accessToken);

    [HttpGet]
    public async Task<List<SearchItem>> SearchAlbum(string query, string accessToken) => 
        await _spotifyClient.SearchAlbum(query, accessToken);

    [HttpGet]
    public async Task<SpotifyAlbum> GetAlbum(string albumId, string accessToken) => 
        await _spotifyClient.GetAlbum(albumId, accessToken);

    [HttpGet]
    public async Task<SpotifyArtist> GetArtist(string artistId, string accessToken) => 
        await _spotifyClient.GetArtist(artistId, accessToken);

    [HttpGet]
    public async Task<List<GetArtistAlbumsItem>> GetArtistAlbums(string artistId, string type, string accessToken) =>
        await _spotifyClient.GetArtistAlbums(artistId, type, accessToken);
}