using CommandService.Models.Entities;
using CommandService.Repositories;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data
{
    public static class PrepData
    {
        public static async Task PrepPopulation(IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices.CreateScope();
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            var repo = serviceScope.ServiceProvider.GetService<ICommandRepo>();

            if (grpcClient == null || repo == null)
            {
                Console.WriteLine("--> Required services not found");
                return;
            }

            try
            {
                Console.WriteLine("--> Getting platforms from gRPC service...");
                var platforms = grpcClient.ReturnAllPlatforms();
                await SeedData(repo, platforms);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not get platforms from gRPC service: {ex.Message}");
            }
        }

        private static async Task SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding new platforms...");

            foreach (var plat in platforms)
            {
                if (!repo.IsExternalPlatformExists(plat.ExternalId))
                {
                    Console.WriteLine($"--> Seeding platform {plat.Name}");
                    await repo.CreatePlatform(plat);
                }
                else
                {
                    Console.WriteLine($"--> Platform {plat.Name} already exists");
                }
            }
        }
    }
}
