using AspNetCoreDemo.Models;
using AspNetCoreDemo.Services;

namespace AspNetCoreDemo.Helpers
{
    public class Helper
    {
        private readonly IBeersService _beersService;
        private readonly IUsersService _usersService;
        public Helper(IUsersService usersService,IBeersService beersService)
        {
            _usersService = usersService;
            _beersService = beersService;
            
        }

        public Beer GetBeerById(int id)
        {
            return _beersService.GetById(id);
        }

        public User GetUserByUsername(string username)
        {
            return _usersService.GetByUsername(username);
        }
    }
}
