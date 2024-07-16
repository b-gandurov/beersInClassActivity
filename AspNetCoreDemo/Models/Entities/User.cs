using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreDemo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        // A one-to-many relationship between User and Beer.
        // One user can create many beers but one beer can only be created by one user.
        public List<Beer> Beers { get; set; } // = new List<Beer>(); Creating an instance of a list is not necessary because
                                              // in the context of EF Core, this is usually not a problem for navigation properties because EF Core will automatically instantiate and populate these properties when loading the entity from the database.

        // A many-to-many relationship between Beer and User.
        // One beer can be rated by many users and one user can rate many beers.
        public List<Rating> Ratings { get; set; }
    }
}
