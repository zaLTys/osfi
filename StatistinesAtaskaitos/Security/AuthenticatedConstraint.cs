using System.Web;
using System.Web.Routing;

namespace UniqTracking.Backend.Web.Security
{
    public class AuthenticatedConstraint : IRouteConstraint
    {
        public bool RequireAnonymous { get; set; }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (RequireAnonymous)
                return !httpContext.Request.IsAuthenticated;
            else
                return httpContext.Request.IsAuthenticated;
        }
    }
}