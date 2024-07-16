using AspNetCoreDemo.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreDemo.Tests
{
    public class TestHelper
    {
        public static Beer GetTestBeer()
        {
            return new Beer
            {
                Id = 1,
                Name = "Beer1",
                Abv = 4.5,
                StyleId = 1,
                Style = new Style { Id = 1, Name = "IPA" },
                UserId = 1,
                User = GetTestUser(),
                Ratings = new List<Rating>
                {
                    new Rating
                    {
                        Id = 1,
                        Value = 5,
                        BeerId = 1,
                        UserId = 1
                    }
                }
            };
        }

        public static BeerRequestDto GetBeerRequestDto()
        {
            return new BeerRequestDto
            {
                Name = "Beer1",
                Abv = 4.5,
                StyleId = 1
            };
        }

        public static BeerResponseDto GetBeerResponseDto()
        {
            return new BeerResponseDto
            {
                Name = "Beer1",
                Abv = 4.5,
                Style = "IPA",
                User = "User1",
                AverageRating = 5,
                Comments = new List<string>()
            };
        }

        public static User GetTestUser()
        {
            return new User { Id = 1, Username = "User1", Password = "password1", IsAdmin = false };
        }

        public static User TestUserAdmin()
        {
            return new User { Id = 3, Username = "AdminUser", IsAdmin = true };
        }
    }
}
