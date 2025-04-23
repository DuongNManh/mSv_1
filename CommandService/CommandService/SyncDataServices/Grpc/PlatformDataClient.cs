using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommandService.Models.Entities;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.SyncDataServices.Grpc
{
    public class PlatformDataClient(IConfiguration configuration, IMapper mapper) : IPlatformDataClient
    {
        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling GRPC Service to get Platforms from {configuration["GrpcPlatform"]}");
            var chanel = GrpcChannel.ForAddress(configuration["GrpcPlatform"]); // create a channel to the gRPC service
            var client = new GrpcPlatform.GrpcPlatformClient(chanel); // create a client for the gRPC service
            var request = new GetAllRequest(); // create a request object

            try
            {
                var reply = client.GetAllPlatforms(request); // call the gRPC service
                return mapper.Map<IEnumerable<Platform>>(reply.Platforms); // map the response to the Platform model
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call GRPC Server: {ex.Message}");
                throw;
            }
        }
    }
}