using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;


namespace RecordStore
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<Models.RecordStoreContext>(null);
        }

        public void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 "TopGenres",                                       // Route name
                 "{controller}/{action}/{id}",                    // URL w/ params
                     new { controller = "Reports", action = "GetGenres", id = "" }  // Param defaults
                   );

            routes.MapRoute(
                "TopArtists",                                       // Route name
                "{controller}/{action}/{id}",                    // URL w/ params
                new { controller = "Reports", action = "GetArtists", id = "" }  // Param defaults
            );
            routes.MapRoute(
                "TopSongs",                                       // Route name
                "{controller}/{action}/{id}",                    // URL w/ params
                new { controller = "Reports", action = "GetSongs", id = "" }  // Param defaults
            );
            routes.MapRoute(
                "TopGenres",                                       // Route name
                "{controller}/{action}/{id}",                    // URL w/ params
                new { controller = "Reports", action = "GetGenres", id = "" }  // Param defaults
            );
        }
    }
}
