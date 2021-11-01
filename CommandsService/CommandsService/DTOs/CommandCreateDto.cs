using System.ComponentModel.DataAnnotations;

namespace CommandsService.DTOs
{
    public class CommandCreateDto
    {
        [Required]
        public string HowTo { get; init; }

        [Required]
        public string CommandLine { get; init; }
    }
}