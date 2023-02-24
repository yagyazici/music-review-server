using AutoMapper;
using MusicReview.Auth;
using MusicReview.Domain.Models.AlbumEntities;
using MusicReview.Domain.DTOs;

namespace MusicReview.Domain.AutoMapper;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile(){
        CreateMap<User, UserProfileDTO>().ReverseMap();
        CreateMap<User, CurrentUserDTO>().ReverseMap();
        CreateMap<Album, AlbumDTO>().ReverseMap();
    }    
}