using AutoMapper;
using EasyID.Server.Models.Dto;
using EasyID.Server.Database.Models;

namespace EasyID.Server.Mappings
{
    internal class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.Groups, opt => opt.Ignore())
                .ForMember(d => d.Roles, opt => opt.Ignore())
                .ForMember(d => d.Permissions, opt => opt.Ignore());
        }
    }
}