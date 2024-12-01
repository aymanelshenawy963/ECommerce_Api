using AutoMapper;
using E_Commerce507Api.DTO;
using E_Commerce507Api.Models;

namespace E_Commerce507Api.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUserDTO, ApplicationUser>().ForMember(dest => dest.UserName, option => option.MapFrom(source => source.Name)).ReverseMap();
        }
    }
}
