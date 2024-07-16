using System.Collections.Generic;
using System.Linq;

using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Repositories;
using Moq;


namespace AspNetCoreDemo.Tests
{

    public class MockBeersRepository
    {
        public Mock<IBeersRepository> GetMockRepository()
        {
            // Create a mock object for IBeersRepository
            var mockRepository = new Mock<IBeersRepository>();

            // Sample data
            var sampleBeers = new List<Beer>
            {
                new Beer
                {
                    Id = 1,
                    Name = "Beer1",
                    Abv = 4.5,
                    StyleId = 1,
                    Style = new Style { Id = 1, Name = "IPA" },
                    UserId = 1,
                    User = new User { Id = 1, Username = "User1" },
                    Ratings = new List<Rating>
                    {
                        new Rating { Id = 1, Value = 5, UserId = 1, User = new User { Id = 1, Username = "User1" } }
                    }
                },
                new Beer
                {
                    Id = 2,
                    Name = "Beer2",
                    Abv = 5.0,
                    StyleId = 2,
                    Style = new Style { Id = 2, Name = "Lager" },
                    UserId = 2,
                    User = new User { Id = 2, Username = "User2" },
                    Ratings = new List<Rating>
                    {
                        new Rating { Id = 2, Value = 4, UserId = 2, User = new User { Id = 2, Username = "User2" } }
                    }
                }
            };

            // Setup for GetAll
            mockRepository.Setup(x => x.GetAll())
                .Returns(sampleBeers);

            // Setup for GetById
            mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int id) => sampleBeers.FirstOrDefault(beer => beer.Id == id));

            // Throw exception when there is no beer with ID
            mockRepository
               .Setup(repo => repo.GetById(It.Is<int>(id => sampleBeers.All(b => b.Id != id))))
               .Throws(new EntityNotFoundException("Beer doesn't exist."));

            // Setup for GetByName
            mockRepository.Setup(x => x.GetByName(It.IsAny<string>()))
                .Returns((string name) => sampleBeers.FirstOrDefault(beer => beer.Name == name));

            // Setup for BeerExists
            mockRepository.Setup(x => x.BeerExists(It.IsAny<string>()))
                .Returns((string name) => sampleBeers.Any(beer => beer.Name == name));

            // Setup for FilterBy
            mockRepository.Setup(x => x.FilterBy(It.IsAny<BeerQueryParameters>()))
                .Returns((BeerQueryParameters parameters) =>
                {
                    var query = sampleBeers.AsQueryable();
                    if (!string.IsNullOrEmpty(parameters.Name))
                    {
                        query = query.Where(beer => beer.Name.Contains(parameters.Name));
                    }
                    if (!string.IsNullOrEmpty(parameters.Style))
                    {
                        query = query.Where(beer => beer.Style.Name.Contains(parameters.Style));
                    }
                    if (parameters.MinAbv.HasValue)
                    {
                        query = query.Where(beer => beer.Abv >= parameters.MinAbv.Value);
                    }
                    if (parameters.MaxAbv.HasValue)
                    {
                        query = query.Where(beer => beer.Abv <= parameters.MaxAbv.Value);
                    }
                    return query.ToList();
                });

            // Setup for Create
            mockRepository.Setup(x => x.Create(It.IsAny<Beer>()))
                .Returns((Beer beer) =>
                {
                    if (sampleBeers.Any(b => b.Name == beer.Name))
                    {
                        throw new DuplicateEntityException("Beer with that name already exists.");
                    }
                    beer.Id = sampleBeers.Max(b => b.Id) + 1;
                    sampleBeers.Add(beer);
                    return beer;
                });

            // Setup for Update
            mockRepository.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Beer>()))
                .Returns((int id, Beer beer) =>
                {
                    var existingBeer = sampleBeers.FirstOrDefault(b => b.Id == id);
                    if (existingBeer != null)
                    {
                        existingBeer.Name = beer.Name;
                        existingBeer.Abv = beer.Abv;
                        existingBeer.StyleId = beer.StyleId;
                        existingBeer.Style = beer.Style;
                        existingBeer.UserId = beer.UserId;
                        existingBeer.User = beer.User;
                        existingBeer.Ratings = beer.Ratings;
                    }
                    return existingBeer;
                });

            // Setup for Delete
            mockRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    var beer = sampleBeers.FirstOrDefault(b => b.Id == id);
                    if (beer != null)
                    {
                        sampleBeers.Remove(beer);
                        return true;
                    }
                    return false;
                });

            return mockRepository;
        }
    }
}
