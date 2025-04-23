namespace CommandService.Models.Responses
{
    public class CommandResponse
    {
        public int Id { get; set; }

        public string HowTo { get; set; }

        public string CommandLine { get; set; }

        public int PlatformId { get; set; }
    }
}
