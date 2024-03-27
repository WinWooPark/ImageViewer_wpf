using ImageViewer.ViewModel;
using Microsoft.Xaml.Behaviors;
using OpenCvSharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ImageViewer.Behaviors
{
    class MouseEventBehavior : Behavior<Image>
    {
        System.Windows.Point _startPoint;
        System.Windows.Point _currentPoint;
        bool _isMouseMove;

        double _scaleStep = 0.1;
        double _mouseSensitivity = 0.8;
        
        double _scale = 1;

        protected override void OnAttached()
        {
            AssociatedObject.MouseWheel += AssociatedObject_MouseWheel;
            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLButtomDown;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLButtomUp;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;

            AssociatedObject.SizeChanged += OnSizeChanged;
        }
    

        protected override void OnDetaching()
        {
            AssociatedObject.MouseWheel -= AssociatedObject_MouseWheel;
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLButtomDown;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLButtomUp;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;

            AssociatedObject.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double canvasWidth = AssociatedObject.ActualWidth;
            double canvasHeight = AssociatedObject.ActualHeight;

        }

        private void AssociatedObject_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _scale += _scaleStep;
                if (_scale > 8) _scale = 8;
            }
            else
            {
                _scale -= _scaleStep;
                if (_scale < 0.9) _scale = 0.9;
            }
            Image image = sender as Image;

            MainViewModel mainViewModel = AssociatedObject.DataContext as MainViewModel;

            double CenterPointX = image.ActualHeight / 2;
            double CenterPointY = image.ActualWidth / 2;

            mainViewModel.Scale = _scale;
            mainViewModel.CenterPointX = CenterPointX;
            mainViewModel.CenterPointY = CenterPointY;

        }

        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isMouseMove == true)
            {
                _currentPoint = e.GetPosition(e.OriginalSource as IInputElement);

                double offsetX = _mouseSensitivity * (_currentPoint.X - _startPoint.X);
                double offsetY = _mouseSensitivity * (_currentPoint.Y - _startPoint.Y);

                MainViewModel mainViewModel = AssociatedObject.DataContext as MainViewModel;

                mainViewModel.TranslateX += offsetX;
                mainViewModel.TranslateY += offsetY;

                _startPoint = _currentPoint;
            }
        }

        private void AssociatedObject_MouseLButtomDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(e.OriginalSource as IInputElement);
            _isMouseMove = true;
        }

        private void AssociatedObject_MouseLButtomUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isMouseMove = false;
        }

    }
}
