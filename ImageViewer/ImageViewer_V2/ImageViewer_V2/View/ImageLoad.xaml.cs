using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageViewer_V2.ViewModel;

namespace ImageViewer_V2.View
{
  
    public partial class ImageLoad : UserControl
    {
        ImageLoadViewModel _imageLoadViewModel;
        public ImageLoad()
        {
            _imageLoadViewModel = new ImageLoadViewModel();
            DataContext = _imageLoadViewModel;
            InitializeComponent();
        }
    }
}
