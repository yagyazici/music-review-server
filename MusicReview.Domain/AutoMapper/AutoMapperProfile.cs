using AutoMapper;
using MusicReview.Domain.Models.AlbumEntities;
using MusicReview.Domain.DTOs;
using MusicReview.Domain.Auth;

namespace MusicReview.Domain.AutoMapper;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile(){
        CreateMap<User, UserProfileDTO>().ReverseMap();
        CreateMap<User, CurrentUserDTO>().ReverseMap();
        CreateMap<Album, AlbumDTO>().ReverseMap();
    }    
}