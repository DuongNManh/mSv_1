namespace PlatformService.Models.EventMessages
{
    public static class EventMessageResponseBuilder
    {
        public static EventMessageResponse<TEvent> Build<TEvent>(string eventName, TEvent data)
        {
            return new EventMessageResponse<TEvent>
            {
                Event = eventName,
                Data = data
            };
        }
    }
}