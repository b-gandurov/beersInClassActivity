using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreDemo.Models
{
    public class Beer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Abv { get; set; }

        // A one-to-many relationship between Style and Beer.
        // One style can have many beers but one beer can only have one style.
        public int StyleId { get; set; } // Foreign key property
        public Style Style { get; set; } // Navigation property

        // A one-to-many relationship between User and Beer.
        // One user can create many beers but one beer can only be created by one user.
        public int UserId { get; set; } // Foreign key property
        public User User { get; set; } // Navigation property

        // A one-to-many relationship between Beer and Rating.
        // One beer can be rated by many users and one user can rate many beers.
        public List<Rating> Ratings { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            Beer otherBeer = (Beer)obj;
            return this.Id == otherBeer.Id &&
                   this.Name == otherBeer.Name &&
                   this.Abv == otherBeer.Abv &&
                   this.StyleId == otherBeer.StyleId &&
                   this.UserId == otherBeer.UserId;
        }
    }
}
