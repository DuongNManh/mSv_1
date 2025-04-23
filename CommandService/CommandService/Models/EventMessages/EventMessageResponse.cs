namespace CommandService.Models.EventMessages
{
    public class EventMessageResponse<T>
    {
        public string Event { get; set; } = default!;
        public T Data { get; set; } = default!;
    }

}
