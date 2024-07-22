using AspNetCoreDemo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;

namespace AspNetCoreDemo.Helpers
{
    public class IsAuhenticatedAttribute : Attribute, IAuthorizationFilter
    {
        public IsAuhenticatedAttribute()
        {
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var beersRepository = ServiceLocator.GetService<IBeersRepository>();
            var usersRepository = ServiceLocator.GetService<IUsersRepository>();
            var isAuthenticated = context.HttpContext.Session.Keys.Contains("CurrentUser");
            if (!isAuthenticated)
            {
                context.Result = new RedirectToRouteResult(new {controller = "Authentication",Action = "Login"});
                return;
            }

            ////if (context.RouteData.Values["id"])
            //var beerId = int.Parse(context.RouteData.Values["id"].ToString());
            //var beer = beersRepository.GetById(beerId);
            //var userName = context.HttpContext.Session.GetString("CurrentUser");
            //var user = usersRepository.GetByUsername(userName);
            //if (!user.IsAdmin || beer.UserId!=user.Id) {
            //    //ViewData["ErrorMessage"] = "You are not authorized for this action";
            //    //Response.StatusCode = 500;
            //    context.Result = new RedirectToRouteResult(new { View = "Error" });
            //    return;
            //}

        }
    }
}
