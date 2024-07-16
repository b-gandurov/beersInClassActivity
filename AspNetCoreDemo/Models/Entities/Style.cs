using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreDemo.Models
{
    public class Style
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // A one-to-many relationship between Style and Beer.
        // One style can have many beers but one beer can only have one style.
        public List<Beer> Beers { get; set; }
    }
}
