using System.Configuration;
using System.Data;
using System.Windows;
using Prism;


namespace FileSystemWatcherExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Views.MainWindow>();
            // throw new NotImplementedException();
        }

    }

}
