using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using Ticketmaster.FizzBuzz.Rules;

namespace Ticketmaster.FizzBuzz.Api.App_Start
{
    public class DI
    {
        public static IContainer RegisterTypes()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();

            builder.RegisterType<FizzBuzzRuleRepository>().As<IFizzBuzzRuleRepository>().SingleInstance();
            builder.RegisterType<FizzBuzzRuleFactory>().As<IFizzBuzzRuleFactory>();
            builder.RegisterType<FizzBuzzCalculator>().As<IFizzBuzzCalculator>();


            return builder.Build();
        }
    }
}