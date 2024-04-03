using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

using ImageViewer_V2.Model.ManagementSystem;
using OpenCvSharp;
using System.Windows;

namespace ImageViewer_V2.Behaviors
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
            _mainSystem.GetImageContolSize(Width, Height);
        }
    }

    public class CanvasSizeBehavior : Behavior<Canvas>
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
            _mainSystem.GetCanvasContolSize(Width, Height);
        }
    }
}
