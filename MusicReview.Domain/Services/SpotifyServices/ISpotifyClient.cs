using MusicReview.Domain.Models.Spotify.Common;
using MusicReview.Domain.Models.Spotify.CurrentSong;
using MusicReview.Domain.Models.Spotify.GetArtistAlbums;
using MusicReview.Domain.Models.Spotify.Search;

namespace MusicReview.Domain.SpotifyServices;

public interface ISpotifyClient
{
    Task<SpotifyCurrentSong> CurrentSong(string accessToken);
    Task<List<SearchItem>> SearchAlbum(string query, string accessToken);
    Task<SpotifyAlbum> GetAlbum(string albumId, string accessToken);
    Task<SpotifyArtist> GetArtist(string artistId, string accessToken);
    Task<List<GetArtistAlbumsItem>> GetArtistAlbums(string artistId, string type, string accessToken);
    Task<List<SearchItem>> GetNewReleases(string accessToken);
}