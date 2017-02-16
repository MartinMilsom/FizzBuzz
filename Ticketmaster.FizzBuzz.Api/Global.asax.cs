using Autofac.Integration.WebApi;
using System.Web.Http;
using Ticketmaster.FizzBuzz.Api.App_Start;

namespace Ticketmaster.FizzBuzz.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var dependencyResolver = new AutofacWebApiDependencyResolver(DI.RegisterTypes());
                    GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
