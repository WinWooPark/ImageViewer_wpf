using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer_V2.Model.MainSystem;
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
            double Width = AssociatedObject.ActualWidth;
            double Height = AssociatedObject.ActualHeight;
        }
    }
}
