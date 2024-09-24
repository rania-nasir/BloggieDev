using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ASP.NET_MVC_Application.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetFullName(this IIdentity identity)
        {
            var userId = identity.GetUserId();
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);

            if (user != null)
            {
                return $"{user.FirstName} {user.LastName}";
            }

            return string.Empty;
        }
    }
}