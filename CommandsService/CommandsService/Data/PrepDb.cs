﻿using System;
using CommandsService.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

            var platforms = grpcClient?.ReturnAllPlatforms();

            SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding new platforms...");

            foreach (var platform in platforms)
            {
                if (!repo.ExternalPlatformExists(platform.ExternalId))
                {
                    repo.CreatePlatform(platform);
                }

                repo.SaveChanges();
            }
        }
    }
}
