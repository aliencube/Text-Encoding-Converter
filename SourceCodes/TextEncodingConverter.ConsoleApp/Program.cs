using System.Diagnostics;
using Aliencube.TextEncodingConverter.Services;
using Aliencube.TextEncodingConverter.Services.Interfaces;
using Autofac;
using System;
using System.Reflection;

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
            Splash();

            using (var scope = _container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IConverterService>();

                try
                {
                    service.Convert();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ooops!");
                    Console.WriteLine();
                    Console.WriteLine("    {0}", ex.Message);
                }
            }
        }

        private static void Splash()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            Console.WriteLine();
            Console.WriteLine("{0} {1}", assembly.GetName().Name, fvi.FileVersion);
            Console.WriteLine();
        }
    }
}