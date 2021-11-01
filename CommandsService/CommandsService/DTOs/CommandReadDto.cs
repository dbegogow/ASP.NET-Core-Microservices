namespace CommandsService.DTOs
{
    public class CommandReadDto
    {
        public int Id { get; init; }

        public string HowTo { get; init; }

        public string CommandLine { get; init; }

        public int PlatformId { get; init; }
    }
}
