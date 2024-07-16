using AspNetCoreDemo.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreDemo.Helpers
{
    public class ModelMapper
    {
        public virtual Beer Map(BeerRequestDto dto)
        {
            return new Beer
            {
                Name = dto.Name,
                Abv = dto.Abv,
                StyleId = dto.StyleId
            };
        }

        public virtual BeerResponseDto Map(Beer beer)
        {
            return new BeerResponseDto()
            {
                Name = beer.Name,
                Abv = beer.Abv,
                Style = beer.Style?.Name,
                User = beer.User.Username,
                AverageRating = beer.Ratings.Any() ? beer.Ratings.Average(rating => rating.Value) : default,
                Comments = beer.Ratings.Select(this.ConvertRatingToComment).ToList()
            };
        }

        private string ConvertRatingToComment(Rating rating)
        {
            var userName = rating.User.Username;
            var beerName = rating.Beer.Name;
            var styleName = rating.Beer.Style.Name;
            var ratingValue = rating.Value;

            var comments = new Dictionary<int, string>
            {
                { 1, $"{userName} had a really bad experience with {beerName} ({styleName}). It was a major disappointment." },
                { 2, $"{userName} thought {beerName} ({styleName}), was just barely drinkable. Not something they'd try again." },
                { 3, $"{userName} found {beerName} ({styleName}), to be quite average. Nothing special, but not terrible either." },
                { 4, $"{userName} really enjoyed {beerName} ({styleName}). It's something they'd happily drink again." },
                { 5, $"{userName} absolutely loved {beerName} ({styleName}). It's a top-notch brew that they'd highly recommend." }
            };

            if (comments.TryGetValue(ratingValue, out var userComment))
            {
                return $"{userComment} Rating: {ratingValue}/5.";
            }
            else
            {
                // Handle unexpected rating values gracefully
                return $"{userName} gave an unusual rating of {ratingValue} for {beerName}, a {styleName}.";
            }
        }

        
    }
}
