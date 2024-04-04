using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows;
using ImageView.Model.ManagementSystem;
using ImageView.Define;


namespace ImageView.Behaviors
{
    public class MouseEventBehavior : Behavior<Image>
    {
        System.Windows.Point _startPoint;
        System.Windows.Point _currentPoint;
        bool _isMouseMove;

        protected override void OnAttached()
        {
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
            Image image = sender as Image;

            MainSystem _mainSystem = MainSystem.Instance;
            _mainSystem.ImageScaleChange(e.Delta, image.ActualWidth, image.ActualHeight);
        }

        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isMouseMove == true)
            {
                _currentPoint = e.GetPosition(e.OriginalSource as IInputElement);

                double offsetX = CommonDefine.MouseSensitivity * (_currentPoint.X - _startPoint.X);
                double offsetY = CommonDefine.MouseSensitivity * (_currentPoint.Y - _startPoint.Y);

                MainSystem _mainSystem = MainSystem.Instance;
                _mainSystem.ImageTranslationChange(offsetX, offsetY);

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
