using Application.Requests.Users;
using Application.Responses.Users;
using AutoMapper;
using Domain.Entities;
using Google.Apis.Auth;

namespace Application.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            User();
        }

        private void User()
        {
            CreateMap<AppUser, UserResponse>();

            CreateMap<GoogleJsonWebSignature.Payload, AppUser>()
                .ForMember(p => p.EmailConfirmed, v => v.MapFrom(p => p.EmailVerified))
                .ForMember(p => p.UserName, v => v.MapFrom(p => p.Email))
                .ForMember(p => p.Name, v => v.MapFrom(p => $"{p.GivenName} {p.FamilyName}"));

            CreateMap<PostUserRequest, AppUser>()
                .ForMember(p => p.UserName, v => v.MapFrom(p => p.Email));
        }
    }
}