using Aliencube.TextEncodingConverter.Services;
using Aliencube.TextEncodingConverter.Services.Interfaces;
using Autofac;

namespace Aliencube.TextEncodingConverter.ConsoleApp
{
    internal class Program
    {
        private static IContainer _container;

        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.Register(p => new ParameterService(args)).As<IParameterService>().PropertiesAutowired();
            builder.RegisterType<ConverterService>().As<IConverterService>().PropertiesAutowired();

            _container = builder.Build();

            Execute();
        }

        private static void Execute()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IConverterService>();
                service.Convert();
            }
        }
    }
}