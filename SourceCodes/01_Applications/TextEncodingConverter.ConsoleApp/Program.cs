using Aliencube.TextEncodingConverter.Services;
using Aliencube.TextEncodingConverter.Services.Interfaces;
using Autofac;
using System;
using System.Diagnostics;
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

        /// <summary>
        /// Executes the application.
        /// </summary>
        private static void Execute()
        {
            Splash();

            using (var scope = _container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IConverterService>();

                try
                {
                    service.Convert(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ooops!");
                    Console.WriteLine();
                    Console.WriteLine("    {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Displays the splash message.
        /// </summary>
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