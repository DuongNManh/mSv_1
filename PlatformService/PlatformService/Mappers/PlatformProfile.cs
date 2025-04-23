using AutoMapper;
using PlatformService.Models.Entities;
using PlatformService.Models.Requests;
using PlatformService.Models.Responses;
using PlatformService.Repositories;

namespace PlatformService.Mappers
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<PlatformCreateRequest, Platform>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PlatformUpdateRequest, Platform>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Platform, PlatformResponse>();

            CreateMap<PlatformResponse, PlatformPublishRequest>();

            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
        }
    }
}
