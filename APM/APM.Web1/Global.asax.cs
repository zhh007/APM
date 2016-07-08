using APM.Notice;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace APM.Web1
{
    public class MvcApplication : System.Web.HttpApplication, ISysNoticeServiceCallback
    {
        public static SysNoticeServiceAgent SNSAgent = null;

        public void ReceiveNotice(string name, string data)
        {
            Debug.WriteLine("APM.Web1 ReceiveNotice -> {1} , {2}", name, data);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SNSAgent = new SysNoticeServiceAgent(new System.ServiceModel.InstanceContext(this));
            SNSAgent.OnLineAsync();


        }
    }
}
