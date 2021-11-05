using AutoMapper;
using PlatfromService;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(p => p.ExternalId, options =>
                    options.MapFrom(pp => pp.Id));
            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(p => p.ExternalId, options =>
                    options.MapFrom(gp => gp.PlatformId))
                .ForMember(p => p.Name, options =>
                    options.MapFrom(gp => gp.Name))
                .ForMember(p => p.Commands, options =>
                    options.Ignore());
        }
    }
}
