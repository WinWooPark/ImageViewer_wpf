
using System.Windows;
using System.Windows.Controls;
using ImageViewer.Model;
using ImageViewer.ViewModel;
using Microsoft.Xaml.Behaviors;

namespace ImageViewer.Behaviors
{
    public class GirdSizeBehavior : Behavior<Image>
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
            double canvasWidth = AssociatedObject.ActualWidth;
            double canvasHeight = AssociatedObject.ActualHeight;
        }
    }
}
