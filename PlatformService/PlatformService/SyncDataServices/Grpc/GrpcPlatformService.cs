using AutoMapper;
using Grpc.Core;
using PlatformService.Repositories;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService(IPlatformRepo platformRepo, IMapper mapper) : GrpcPlatform.GrpcPlatformBase
    {
        public override Task<GrpcPlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new GrpcPlatformResponse();
            var platforms = platformRepo.GetAllPlatforms().Result;

            foreach (var plat in platforms)
            {
                response.Platforms.Add(mapper.Map<GrpcPlatformModel>(plat));
            }
            return Task.FromResult(response);
        }
    }
}