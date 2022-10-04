// using System.Reflection.Emit;
    using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// using System.Threading;
// using System.Threading.Tasks;

    
    namespace CommandService.Models {
        public class AppDbContext : DbContext
         {
            public AppDbContext (DbContextOptions<AppDbContext> options) : base (options) { }


            
            public DbSet<Command> Commands { get; set; }
            public DbSet<Platform> Platforms { get; set; }

            protected override void OnModelCreating (ModelBuilder modelBuilder){

                modelBuilder
                .Entity<Platform>()
                .HasMany(p=>p.Commands)
                .WithOne(p=>p.Platform!)
                .HasForeignKey(p=>p.PlatformId);


                modelBuilder
                .Entity<Command>()
                .HasOne(p=>p.Platform!)
                .WithMany(p=>p.Commands)
                .HasForeignKey(p=>p.PlatformId);


            }
        }
    }
    
            

    