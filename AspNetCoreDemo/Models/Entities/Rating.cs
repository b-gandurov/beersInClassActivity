using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreDemo.Models
{
    // This class represents a many-to-many relationship between Beer and User.
    // One beer can be rated by many users and one user can rate many beers.
    public class Rating
    {
        public int Id { get; set; }

        public int? BeerId { get; set; } // Foreign key property
        public Beer Beer { get; set; } // Navigation property

        public int? UserId { get; set; } // Foreign key property
        public User User { get; set; } // Navigation property

        // This property represents the rating value given by a user to a beer.
        [Range(1, 5)]
        public int Value { get; set; }
    }
}
