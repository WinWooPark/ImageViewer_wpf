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
        double _mouseSensitivity = 0.5;
        
        double _scale = 0.5;

        private TransformGroup _transformGroup;
        private ScaleTransform _sclaeTransform;
        private TranslateTransform _translateTransform;


        protected override void OnAttached()
        {
            //_transformGroup = new TransformGroup();
            //_sclaeTransform = new ScaleTransform();
            //_translateTransform = new TranslateTransform();

            //_transformGroup.Children.Add(_translateTransform);
            //_transformGroup.Children.Add(_sclaeTransform);

            //AssociatedObject.RenderTransform = _transformGroup;

            AssociatedObject.MouseWheel += AssociatedObject_MouseWheel;
            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLButtomDown;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLButtomUp;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }
    

        protected override void OnDetaching()
        {
            AssociatedObject.MouseWheel -= AssociatedObject_MouseWheel;
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLButtomDown;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLButtomUp;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
        }

        private void AssociatedObject_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _scale += _scaleStep;
                if (_scale > 3) _scale = 3;
            }
            else
            {
                _scale -= _scaleStep;
                if (_scale < 0.2) _scale = 0.2;
            }
            
            MainViewModel mainViewModel = AssociatedObject.DataContext as MainViewModel;
            mainViewModel.Scale = _scale;

            //_sclaeTransform.ScaleX = _scale;
            //_sclaeTransform.ScaleY = _scale;
        }

        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isMouseMove == true)
            {
                _currentPoint = e.GetPosition(e.OriginalSource as IInputElement);

                double offsetX = _mouseSensitivity * (_currentPoint.X - _startPoint.X);
                double offsetY = _mouseSensitivity * (_currentPoint.Y - _startPoint.Y);

                //_translateTransform.X += offsetX;
                //_translateTransform.Y += offsetY;

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
