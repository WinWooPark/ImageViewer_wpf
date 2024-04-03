using ImageViewer_V2.ViewModel;
using ImageViewer_V2.Model.ManagementSystem;
using System.Windows;

namespace ImageViewer_V2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainViewModel _mainViewModel;
        MainSystem _mainSystem;
        public App()
        {
            _mainSystem = MainSystem.Instance;
            _mainSystem.InitMainSystem();

            _mainViewModel = new MainViewModel();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
               DataContext = _mainViewModel
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }

}
