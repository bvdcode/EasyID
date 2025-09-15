using AutoMapper;
using EasyID.Server.Models.Dto;
using EasyID.Server.Database.Models;

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