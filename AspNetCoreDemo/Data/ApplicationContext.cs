using System.Collections.Generic;

using AspNetCoreDemo.Models;

using Microsoft.EntityFrameworkCore;

namespace AspNetCoreDemo.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Beer> Beers { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<Style> styles = new List<Style>()
            {
                new Style
                {
                    Id = 1,
                    Name = "Special Ale"
                },
                new Style
                {
                    Id = 2,
                    Name = "English Porter"
                },
                new Style
                {
                    Id = 3,
                    Name = "Indian Pale Ale"
                }
            };

            List<User> users = new List<User>()
            {
                new User
                {
                    Id = 1,
                    Username = "Admin",
                    Password = "MTIz" /* 123 in base64 */,
                    IsAdmin = true
                },
                new User
                {
                    Id = 2,
                    Username = "Alice",
                    Password = "MTIz" /* 123 in base64 */,
                    IsAdmin = false
                },
                new User
                {
                    Id = 3,
                    Username = "Bob",
                    Password = "MTIz" /* 123 in base64 */,
                    IsAdmin = false
                }
            };

            List<Beer> beers = new List<Beer>()
            {
                new Beer
                {
                    Id = 1,
                    Name = "Glarus English Ale",
                    Abv = 4.6,
                    StyleId = styles[0].Id, /* Special Ale */
                    UserId = users[1].Id /* Alice */
                },
                new Beer
                {
                    Id = 2,
                    Name = "Rhombus Porter",
                    Abv = 5.0,
                    StyleId = styles[1].Id, /* English Porter */
                    UserId = users[1].Id /* Alice */ 
                },
                new Beer
                {
                    Id = 3,
                    Name = "Opasen Char",
                    Abv = 6.6,
                    StyleId = styles[2].Id, /* Indian Pale Ale */
                    UserId = users[2].Id /* Bob */
                }
            };

            List<Rating> ratings = new List<Rating>()
            {
                new Rating()
                {
                    Id = 1,
                    UserId = users[2].Id /* Bob */,
                    BeerId = beers[0].Id /* Glarus English Ale */,
                    Value = 5 /* Bob likes ales */
                },
                new Rating()
                {
                    Id = 2,
                    UserId = users[1].Id /* Alice */,
                    BeerId = beers[0].Id /* Glarus English Ale */,
                    Value = 2 /* Alice doesn't like ales */
                },
                new Rating()
                {
                    Id = 3,
                    UserId = users[2].Id /* Bob */,
                    BeerId = beers[1].Id /* Rhombus Porter */,
                    Value = 1 /* Bob doesn't like porters */
                },
                new Rating()
                {
                    Id = 4,
                    UserId = users[1].Id /* Alice */,
                    BeerId = beers[1].Id /* Rhombus Porter */,
                    Value = 3 /* Alice likes porters */
                },
                new Rating()
                {
                    Id = 5,
                    UserId = users[2].Id /* Bob */,
                    BeerId = beers[2].Id /* Opasen Char */,
                    Value = 5 /* Bob likes IPAs */
                },
                new Rating()
                {
                    Id = 6,
                    UserId = users[1].Id /* Alice */,
                    BeerId = beers[2].Id /* Opasen Char */,
                    Value = 5 /* Alice likes IPAs */
                }
            };

            // Seed the database with data
            modelBuilder.Entity<Style>().HasData(styles);
            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Beer>().HasData(beers);
            modelBuilder.Entity<Rating>().HasData(ratings);
        }
    }
}