using Microsoft.AspNetCore.Mvc;

namespace dotnetCologne.RichardsonMaturityModel.Api.Infrastructure
{

    public static class ControllerExtensions
    {

         public static IActionResult ConflictWithRoute(this Controller controller, string routeName, object routeValues)
        {
            // Allow to include route to conflicting resource
            return new ConflictAtRouteResult(routeName, routeValues);
        }

    }

}