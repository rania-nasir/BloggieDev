using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ASP.NET_MVC_Application
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // Application error event: Handles errors globally.
        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;

            if (httpException != null)
            {
                if (httpException.GetHttpCode() == 404)
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.Redirect("/Error/NotFound");
                }
            }
        }

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);

            // If the status is 401 (unauthorized) and the user is authenticated, treat it as 403 (forbidden)
            if (context.Response.StatusCode == 401 && User.Identity.IsAuthenticated)
            {
                context.Response.ClearContent();
                context.Response.StatusCode = 403;  // Set the status code to 403 Forbidden
                context.Response.RedirectToRoute(new { controller = "Error", action = "Forbidden" });
            }
        }

    }
}
