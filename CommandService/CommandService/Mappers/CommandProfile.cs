using AutoMapper;
using CommandService.Models.Entities;
using CommandService.Models.Requests;
using CommandService.Models.Responses;

namespace CommandService.Mappers
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            CreateMap<CommandCreateRequest, Command>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PlatformId, opt => opt.Ignore())
                .ForMember(dest => dest.Platform, opt => opt.Ignore());

            CreateMap<CommandUpdateRequest, Command>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PlatformId, opt => opt.Ignore())
                .ForMember(dest => dest.Platform, opt => opt.Ignore());

            CreateMap<Command, CommandResponse>();
        }
    }
}
