using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Command
    {
        public int Id { get; set; }

        [Required]
        public string HowTo { get; set; }

        [Required]
        public string CommandLine { get; set; }
        
        public int PlatformId { get; set; }

        public Platform Platform { get; set; }
    }
}
