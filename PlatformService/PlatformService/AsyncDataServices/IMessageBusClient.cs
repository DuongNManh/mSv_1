using PlatformService.Models.Requests;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishRequest plat);
    }
}
