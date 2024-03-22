using ImageViewer.Model.MainSystem;
using ImageViewer.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using ImageViewer.Model;

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
            IntegratedClass.Instance.SystemInfo = _systemInfo;

            _mainViewModel = new MainViewModel();
            IntegratedClass.Instance.MainViewModel = _mainViewModel;
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
            _systemInfo.CloseSystemInfo();
            base.OnExit(e);
        }
    }

}
