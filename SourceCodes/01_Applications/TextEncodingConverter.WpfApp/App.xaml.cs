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

            var container = builder.Build();
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);

            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }
    }
}