using System;
using PlatformService.Models;
using System.Collections.Generic;
using System.Linq;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatformRepo(AppDbContext context)
            => this._context = context;

        public bool SaveChanges()
            => this._context.SaveChanges() >= 0;

        public IEnumerable<Platform> GetAllPlatforms()
            => this._context
                .Platforms
                .ToList();

        public Platform GetPlatformById(int id)
            => this._context
                .Platforms
                .FirstOrDefault(p => p.Id == id);

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            this._context
                .Platforms
                .Add(platform);
        }
    }
}
