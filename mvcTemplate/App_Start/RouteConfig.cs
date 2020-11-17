using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Realtek.IntraLogin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var lang = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{language}",
                defaults: new
                {
                    language = lang,
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }
             );

        }
    }
}
