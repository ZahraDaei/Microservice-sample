using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto,Command>();
            CreateMap<Command,CommandReadDto>();
            CreateMap<PlatformModel,Platform>()
            .ForMember(des=>des.ExternalID,opt=>opt.MapFrom(opt=>opt.Id));
        }
    }
}