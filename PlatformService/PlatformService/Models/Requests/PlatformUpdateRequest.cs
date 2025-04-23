using System.ComponentModel.DataAnnotations;

namespace PlatformService.Models.Requests
{
    public class PlatformUpdateRequest
    {
        [Required]
        [MaxLength(255, ErrorMessage = "Platform name cannot exceed 255 characters.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Publisher name cannot exceed 255 characters.")]
        public string Publisher { get; set; }

        [Required]
        public string Cost { get; set; }
    }
}
