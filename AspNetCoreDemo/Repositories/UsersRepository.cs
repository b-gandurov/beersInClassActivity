using System.Collections.Generic;
using System.Linq;

using AspNetCoreDemo.Data;
using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Models;

using Microsoft.EntityFrameworkCore;

namespace AspNetCoreDemo.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationContext context;

        public UsersRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public List<User> GetAll()
        {
            return this.GetUsers().ToList();
        }

        public User GetById(int id)
        {
            User user = this.GetUsers().FirstOrDefault(u => u.Id == id);

            return user ?? throw new EntityNotFoundException($"User with id={id} doesn't exist.");
        }

        public User GetByUsername(string username)
        {
            User user = this.GetUsers().FirstOrDefault(u => u.Username == username);

            return user ?? throw new EntityNotFoundException($"User with username={username} doesn't exist.");
        }

        private IQueryable<User> GetUsers()
        {
            return this.context.Users
                    .Include(user => user.Ratings)
                        .ThenInclude(rating => rating.Beer)
                            .ThenInclude(beer => beer.Style);
        }
    }
}
