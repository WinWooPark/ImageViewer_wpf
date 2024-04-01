
using System.Windows;
using System.Windows.Controls;
using ImageViewer.Model;
using ImageViewer.ViewModel;
using Microsoft.Xaml.Behaviors;
using OpenCvSharp;

namespace ImageViewer.Behaviors
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
            double Width = AssociatedObject.ActualWidth;
            double Height = AssociatedObject.ActualHeight;

            IntegratedClass.Instance.ImageControlSize = new Size2d(Width, Height);
            IntegratedClass.Instance.CalRatio();
        }
    }

    public class BorderSizeBehavior : Behavior<Canvas>
    {
        System.Windows.Point _startPoint;
        System.Windows.Point _currentPoint;
        bool _isMouseMove;

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
            double Width = AssociatedObject.ActualWidth;
            double Height = AssociatedObject.ActualHeight;

            IntegratedClass.Instance.CanvasControlSize = new Size2d(Width, Height);
        }

        private void AssociatedObject_MouseLButtomDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(e.OriginalSource as IInputElement);
            _isMouseMove = true;
        }
    }
}
