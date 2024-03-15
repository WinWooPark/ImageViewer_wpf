﻿using ImageViewer.MainSystem;
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
            //여기에다 모델 생성
            _systemInfo = new SystemInfo();
        }
        protected override void OnStartup(StartupEventArgs e) 
        {
            _mainViewModel = new MainViewModel(_systemInfo);

            MainWindow = new MainWindow()
            {
                DataContext = _mainViewModel
            };
            MainWindow.Show();

            _systemInfo.SetViewModel(_mainViewModel);

            base.OnStartup(e);
        }
    }

}
