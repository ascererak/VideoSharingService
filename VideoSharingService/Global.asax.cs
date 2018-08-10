using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VideoSharingService.Business;
using VideoSharingService.Data;

namespace VideoSharingService
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

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var error = new Data.Entities.Error() { ExceptionType = ex.GetType().ToString(), ExceptionText = ex.Message };
            var helper = new UserHelper(new Repository());

            // Save error into DB
            helper.AddError(error);
        }
    }
}
