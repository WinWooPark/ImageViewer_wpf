using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageViewer_V2.Model.MainSystem;

namespace ImageViewer_V2.ViewModel
{
    public class ImageViewerViewModel : ObservableObject
    {
        IntegratedClass _integratedClass;
        MainSystem _mainSystem;

        public ImageViewerViewModel()
        {  
            _mainSystem = MainSystem.Instance;
            _mainSystem.ViewModels.Add(this.GetType().Name, this);
            _integratedClass = IntegratedClass.Instance;
            
            Createcommand();
        }

        BitmapSource _mainImage;
        public BitmapSource MainImage
        {
            get { return _mainImage; }
            set 
            {
                if (_mainImage != value) 
                {
                    _mainImage = value;
                    OnPropertyChanged(nameof(MainImage));
                }
            } 
        }

        BitmapSource _subImage;
        public BitmapSource SubImage
        {
            get { return _subImage; }
            set
            {
                if (_subImage != value)
                {
                    _subImage = value;
                    OnPropertyChanged(nameof(_subImage));
                }
            }
        }

        public void UpDateMainImage(BitmapSource InputImage)
        {
            MainImage = InputImage;
        }

        public void UpDateSubImage(BitmapSource InputImage)
        {
            SubImage = InputImage;
        }

        public void UpDateEllipses()
        {

        }

        public RelayCommand ExitCommand { get; set; }

        void Createcommand() 
        {
            ExitCommand = new RelayCommand(_mainSystem.ExitProgram);
         }
    }
}
