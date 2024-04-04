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

            _mainSystem.GetCanvasControlSize(Width, Height);
        }
    }
}
