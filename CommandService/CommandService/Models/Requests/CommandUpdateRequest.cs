using System.ComponentModel.DataAnnotations;

namespace CommandService.Models.Requests
{
    public class CommandUpdateRequest
    {
        [Required(ErrorMessage = "HowTo is required")]
        [Length(1, 255, ErrorMessage = "HowTo must be between 1 and 255 characters.")]
        public string HowTo { get; set; }

        [Required(ErrorMessage = "CommandLine is required")]
        [Length(1, 255, ErrorMessage = "CommandLine must be between 1 and 255 characters.")]
        public string CommandLine { get; set; }
    }
}
