using Aliencube.TextEncodingConverter.Configs;
using Aliencube.TextEncodingConverter.Configs.Interfaces;
using Aliencube.TextEncodingConverter.Services;
using Aliencube.TextEncodingConverter.Services.Interfaces;
using Aliencube.TextEncodingConverter.ViewModels;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using System.Windows;

namespace Aliencube.TextEncodingConverter.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            builder.Register(p => TextEncodingConverterSettings.CreateInstance()).As<ITextEncodingConverterSettings>().InstancePerLifetimeScope();
            builder.RegisterType<ParameterService>().As<IParameterService>().InstancePerLifetimeScope();
            builder.RegisterType<ConverterService>().As<IConverterService>().InstancePerLifetimeScope();
            builder.RegisterType<MainWindowViewModel>().InstancePerLifetimeScope();

            var container = builder.Build();

            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);

            Current.MainWindow = new MainWindow(ServiceLocator.Current.GetInstance<ITextEncodingConverterSettings>(),
                                                ServiceLocator.Current.GetInstance<IConverterService>(),
                                                ServiceLocator.Current.GetInstance<MainWindowViewModel>());
            Current.MainWindow.Show();
        }
    }
}