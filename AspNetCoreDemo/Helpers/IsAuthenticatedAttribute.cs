using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreDemo.Services;


namespace AspNetCoreDemo.Helpers
{
    public class IsAuthenticatedAttribute : Attribute, IAuthorizationFilter
    {
        private IBeersService _beerService;
        private IUsersService _userService;
        public IsAuthenticatedAttribute()
        {
            
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                _beerService = context.HttpContext.RequestServices.GetService<IBeersService>();
                _userService = context.HttpContext.RequestServices.GetService<IUsersService>();

                var isAuthenticated = context.HttpContext.Session.Keys.Contains("CurrentUser");
                if (!isAuthenticated)
                {
                    context.Result = new RedirectToRouteResult(new { controller = "Authentication", Action = "Login" });
                    return;
                }

                var beerId = int.Parse(context.RouteData.Values["id"].ToString());
                var beer = _beerService.GetById(beerId);
                var userName = context.HttpContext.Session.GetString("CurrentUser");
                var user = _userService.GetByUsername(userName);

                if (!user.IsAdmin && beer.UserId != user.Id)
                {
                    throw new UnauthorizedOperationException("You are not authorized to perform this action.");
                }
            }
            catch (UnauthorizedOperationException ex)
            {
                HandleException(context, ex, 403);
            }
            catch (EntityNotFoundException ex)
            {
                HandleException(context, ex, 404);
            }
            catch (Exception ex)
            {
                HandleException(context, ex, 500);
            }
        }

        private void HandleException(AuthorizationFilterContext context, Exception ex, int statusCode)
        {
            context.HttpContext.Response.StatusCode = statusCode;
            var result = new ViewResult { ViewName = "Error" };
            result.ViewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                context.ModelState
            )
            {
                ["ErrorMessage"] = ex.Message
            };
            context.Result = result;
        }
    }
}
