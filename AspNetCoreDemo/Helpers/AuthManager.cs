using AspNetCoreDemo.Exceptions;
using System.Text;
using System;
using AspNetCoreDemo.Services;
using AspNetCoreDemo.Models;

namespace AspNetCoreDemo.Helpers
{
    public class AuthManager
    {
        private const string InvalidCredentialsErrorMessage = "Invalid credentials!";

        private readonly IUsersService usersService;

        public AuthManager(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public virtual User TryGetUser(string credentials)
        {
            string[] credentialsArray = credentials.Split(':');
            string username = credentialsArray[0];
            string password = credentialsArray[1];

            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            try
            {
                var user = this.usersService.GetByUsername(username);

                if (user.Password != encodedPassword)
                {
                    throw new UnauthorizedOperationException(InvalidCredentialsErrorMessage);
                }

                return user;
            }
            catch (EntityNotFoundException)
            {
                throw new UnauthorizedOperationException(InvalidCredentialsErrorMessage);
            }
        }
    }
}
