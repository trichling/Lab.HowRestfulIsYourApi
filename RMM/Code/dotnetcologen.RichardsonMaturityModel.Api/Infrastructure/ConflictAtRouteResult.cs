using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace dotnetCologne.RichardsonMaturityModel.Api.Infrastructure
{

    
    public class ConflictAtRouteResult : ObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovedPermanentlyToRouteResult"/> class with the values
        /// provided.
        /// </summary>
        /// <param name="routeName"></param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        public ConflictAtRouteResult(string routeName, object routeValues)
            : this(routeName: routeName, routeValues: routeValues, value: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovedPermanentlyToRouteResult"/> class with the values
        /// provided.
        /// </summary>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="value">The value to format in the entity body.</param>
        public ConflictAtRouteResult(
            string routeName,
            object routeValues,
            object value)
            : base(value)
        {
            RouteName = routeName;
            RouteValues = routeValues == null ? null : new RouteValueDictionary(routeValues);
            StatusCode = StatusCodes.Status409Conflict;
        }

        /// <summary>
        /// Gets or sets the <see cref="IUrlHelper" /> used to generate URLs.
        /// </summary>
        public IUrlHelper UrlHelper { get; set; }

        /// <summary>
        /// Gets or sets the name of the route to use for generating the URL.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// Gets or sets the route data to use for generating the URL.
        /// </summary>
        public RouteValueDictionary RouteValues { get; set; }

        /// <inheritdoc />
        public override void OnFormatting(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            base.OnFormatting(context);

            var urlHelper = UrlHelper;
            if (urlHelper == null)
            {
                var services = context.HttpContext.RequestServices;
                urlHelper = services.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context);
            }

            var url = urlHelper.Link(RouteName, RouteValues);

            if (string.IsNullOrEmpty(url))
            {
                throw new InvalidOperationException("No routes matched.");
            }

            context.HttpContext.Response.Headers[HeaderNames.Location] = url;
        }

        [Serializable]
        private class ArgumentNullException : Exception
        {
            public ArgumentNullException()
            {
            }

            public ArgumentNullException(string message) : base(message)
            {
            }

            public ArgumentNullException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ArgumentNullException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }

}