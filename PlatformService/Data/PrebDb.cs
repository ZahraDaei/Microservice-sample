using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Data
{
    public static class PrebDb
    {
        public static void PrebPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {

            if (isProd)
            {
                try
                {
                    Console.WriteLine("-->Attempting to apply migration ...");
                    context.Database.Migrate();

                }
                catch (System.Exception e)
                {

                    Console.WriteLine($"--> Could not run migrations:{e.Message}");
                }
            }
        

                if (!context.Platforms.Any())
                {
                    Console.WriteLine("-->seeding data...");
                    context.Platforms.AddRange(
                        new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                        new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                        new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                        );
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("-->we already have data");

                }
            
        }
    }
}
