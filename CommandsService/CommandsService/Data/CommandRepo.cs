using System;
using System.Linq;
using CommandsService.Models;
using System.Collections.Generic;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
            => this._context = context;

        public bool SaveChanges()
            => this._context.SaveChanges() >= 0;

        public IEnumerable<Platform> GetAllPlatforms()
            => this._context.Platforms
                .ToList();

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            this._context.Platforms.Add(platform);
        }

        public bool PlatformExits(int platformId)
            => this._context.Platforms.Any(p => p.Id == platformId);

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
            => this._context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);

        public Command GetCommand(int platformId, int commandId)
            => this._context.Commands
                .FirstOrDefault(c => c.PlatformId == platformId && c.Id == commandId);

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.PlatformId = platformId;
            this._context.Commands.Add(command);
        }
    }
}
