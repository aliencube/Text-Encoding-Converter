using Aliencube.TextEncodingConverter.Services;
using Aliencube.TextEncodingConverter.Services.Interfaces;
using Autofac;

namespace Aliencube.TextEncodingConverter.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.Register(p => new ParameterService(args)).As<IParameterService>().PropertiesAutowired();
            builder.RegisterType<ConverterService>().As<IConverterService>().PropertiesAutowired();

            using (var container = builder.Build())
            {
                container.Resolve<ConverterService>().Convert();
            }
        }
    }
}