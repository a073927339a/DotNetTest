using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Realtek.IntraLogin.Commons;

namespace Realtek.IntraLogin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string log4netPath = Server.MapPath("~/log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(log4netPath));

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ViewEngines.Engines.Add(new WebFormViewEngine());
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {

            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {
                String cultureInfoName = cultureCookie.Value;
                // set culture
                System.Threading.Thread.CurrentThread.CurrentCulture =
                new System.Globalization.CultureInfo(cultureInfoName);
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                new System.Globalization.CultureInfo(cultureInfoName);

            }
        }
    }
}
