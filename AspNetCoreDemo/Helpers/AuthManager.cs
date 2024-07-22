using AspNetCoreDemo.Exceptions;
using System.Text;
using System;
using AspNetCoreDemo.Services;
using AspNetCoreDemo.Models;
using Polly;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreDemo.Helpers
{
    public class AuthManager
    {
        private const string InvalidCredentialsErrorMessage = "Invalid credentials!";

        private readonly IUsersService usersService;
        private readonly IHttpContextAccessor accessor;
        

        public AuthManager(IUsersService usersService,IHttpContextAccessor accessor)
        {
            this.usersService = usersService;
            this.accessor = accessor;
        }

        public User CurrentUser
        {
            get
            {
                string username = this.accessor.HttpContext.Session.GetString("CurrentUser");
                if (username == null)
                {
                    return null;
                }
                return usersService.GetByUsername(username);
            }
            private set
            {
                User user = value;
                if (user != null)
                {
                    this.accessor.HttpContext.Session.SetString("CurrentUser", user.Username);
                }
                else
                {
                    this.accessor.HttpContext.Session.Remove("CurrentUser");
                }

            }
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
