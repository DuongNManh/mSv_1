using AutoMapper;
using CommandService.Models.Entities;
using CommandService.Models.EventMessages;
using CommandService.Models.Responses;
using CommandService.Repositories;
using System.Text.Json;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _scopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public async Task ProcessEvent(string message)
        {
            // Process the event synchronously  
            Console.WriteLine($"Processing event: {message}");
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    Console.WriteLine("Platform published event processed.");
                    await AddPlatform(message); // Fix: Added 'await' to ensure the method is awaited  
                    break;
                case EventType.CommandCreated:
                    Console.WriteLine("Command created event processed.");
                    break;
                default:
                    Console.WriteLine("Undefined event type. No action taken.");
                    break;
            }
        }

        private EventType DetermineEvent(string notiMessage)
        {
            Console.WriteLine($"Determining event for message: {notiMessage}");
            var eventType = JsonSerializer.Deserialize<GenericEventResponse>(notiMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("Platform published event detected.");
                    return EventType.PlatformPublished;
                case "Command_Created":
                    Console.WriteLine("Command created event detected.");
                    return EventType.CommandCreated;
                default:
                    Console.WriteLine("Undefined event type.");
                    return EventType.Undefined; // Default case
            }
        }

        private async Task AddPlatform(string platformPublicMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPublish = JsonSerializer.Deserialize<EventMessageResponse<PlatformPublicResponse>>(platformPublicMessage);

                if (platformPublish?.Data == null)
                {
                    Console.WriteLine("Platform data is missing in the event message.");
                    return;
                }

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublish.Data);
                    if (!repo.IsExternalPlatformExists(plat.ExternalId))
                    {
                        Console.WriteLine("Platform does not exist in the database. Adding new platform.");
                        await repo.CreatePlatform(plat);
                        Console.WriteLine("Platform added to the database successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Platform already exists in the database.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not add platform to db: {e.Message}");
                    throw;
                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        CommandCreated,
        Undefined
    }
}