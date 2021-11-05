using AutoMapper;
using PlatfromService;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(gp => gp.PlatformId, options =>
                    options.MapFrom(p => p.Id))
                .ForMember(gp => gp.Name, options =>
                    options.MapFrom(p => p.Name))
                .ForMember(gp => gp.Publisher, options =>
                    options.MapFrom(p => p.Publisher));
        }
    }
}