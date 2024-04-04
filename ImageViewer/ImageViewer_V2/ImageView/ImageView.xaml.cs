using System.Windows.Controls;
using ImageView.Model.ManagementSystem;
using ImageView.ViewModel;

namespace ImageView
{
    public partial class ImageView : UserControl
    {
        private readonly ImageViewViewModel _imageViewViewModel;
        private readonly MainSystem _mainSystem;
        public ImageView()
        {
            _mainSystem = MainSystem.Instance;
            _imageViewViewModel = new ImageViewViewModel(_mainSystem);
            
            InitializeComponent();
            this.DataContext = _imageViewViewModel;
        }
    }

}
