using System;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace IsomanagerWeb
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Clear();
            
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Off;
            routes.EnableFriendlyUrls(settings);

            routes.MapPageRoute(
                "Default",
                "",
                "~/Pages/Dashboard.aspx"
            );

            routes.MapPageRoute(
                "Error",
                "Error",
                "~/Pages/Error.aspx"
            );
        }
    }
}
