using System;
using System.Web.Mvc;
using StatistinesAtaskaitos.Security;

namespace UniqTracking.Backend.Web.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticateAttribute : FilterAttribute, System.Web.Mvc.IAuthorizationFilter
    {
        public void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var authProvider = DependencyResolver.Current.GetService<IAuthenticationProvider>();

            authProvider.TryAuthenticate();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
    }
}