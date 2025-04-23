using PlatformService.Models.Responses;
using System.Text;
using System.Text.Json;
namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration) : ICommandDataClient
    {
        public async Task<bool> SendPlatformToCommand(PlatformResponse plat)
        {
            try
            {
                var httpContent = new StringContent(
                    JsonSerializer.Serialize(plat),  // Just the platform data, not wrapped
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await httpClient.PostAsync(
                    $"{configuration["CommandService"]}",
                    httpContent
                );

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"--> Could not send platform to Command Service: {ex.Message}");
                return false;
            }
        }
    }
}
