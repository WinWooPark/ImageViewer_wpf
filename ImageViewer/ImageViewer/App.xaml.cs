using ImageViewer.Model.MainSystem;
using ImageViewer.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SystemInfo _systemInfo;
        private MainViewModel _mainViewModel;

        public App()
        {
            _systemInfo = new SystemInfo();
            _mainViewModel = new MainViewModel(_systemInfo);
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
