using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows;
using ImageView.Model.ManagementSystem;

namespace ImageView.Behaviors
{
    public class ImageSizeBehavior : Behavior<Image>
    {
        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += OnSizeChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainSystem _mainSystem = MainSystem.Instance;

            double Width = AssociatedObject.ActualWidth;
            double Height = AssociatedObject.ActualHeight;

            _mainSystem.GetImageControlSize(Width, Height);
        }
    }

    public class CanvasSizeBehavior : Behavior<Canvas>
    {
        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += OnSizeChanged;

            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLButtomDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= OnSizeChanged;

            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLButtomDown;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainSystem _mainSystem = MainSystem.Instance;

            double Width = AssociatedObject.ActualWidth;
            double Height = AssociatedObject.ActualHeight;

            _mainSystem.GetCanvasControlSize(Width, Height);
        }

        private void AssociatedObject_MouseLButtomDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point st = e.GetPosition(e.OriginalSource as IInputElement);
            bool _isMouseMove = true;
        }
    }
}
