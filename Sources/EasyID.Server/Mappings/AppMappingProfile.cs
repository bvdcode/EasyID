using AutoMapper;
using EasyID.Server.Database;
using EasyID.Server.Models.Dto;

namespace EasyID.Server.Mappings
{
    internal class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}