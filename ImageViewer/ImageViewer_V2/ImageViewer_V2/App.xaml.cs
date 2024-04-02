using ImageViewer_V2.ViewModel;
using System.Windows;

namespace ImageViewer_V2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainViewModel _mainViewModel;

        public App()
        {
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
    }

}
