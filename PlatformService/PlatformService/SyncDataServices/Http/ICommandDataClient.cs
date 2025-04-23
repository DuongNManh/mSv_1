using PlatformService.Models.Responses;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task<bool> SendPlatformToCommand(PlatformResponse plat);
    }
}