using System.Collections.Generic;

namespace AspNetCoreDemo.Models
{
    public class BeerResponseDto
    {
        public string Name { get; set; }
        public double Abv { get; set; }
        public string Style { get; set; }
        public string User { get; set; }
        public double AverageRating { get; set; }
        public List<string> Comments { get; set; }

        override public bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            BeerResponseDto other = (BeerResponseDto)obj;

            return this.Name == other.Name
                && this.Abv == other.Abv
                && this.Style == other.Style
                && this.User == other.User
                && this.AverageRating == other.AverageRating;
        }
    }
}
