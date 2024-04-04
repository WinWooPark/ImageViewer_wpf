using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageViewer_V2.Model.Data;
using ImageViewer_V2.Model.ManagementSystem;

namespace ImageViewer_V2.ViewModel
{
    public class ImageViewerViewModel : ObservableObject
    {
        IntegratedClass _integratedClass;
        MainSystem _mainSystem;
        SystemData _systemData;

        public ImageViewerViewModel()
        {  
            _mainSystem = MainSystem.Instance;
            _integratedClass = _mainSystem.IntegratedClass;
            _integratedClass.ImageViewerViewModel = this;
            _systemData = _integratedClass.SystemData;
            
            Createcommand();
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

        public void UpDateSubImage(BitmapSource InputImage)
        {
            SubImage = InputImage;
        }

        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ImageFitCommand { get; set; }
        void Createcommand() 
        {
            ExitCommand = new RelayCommand(_mainSystem.ExitProgram);
            ImageFitCommand = new RelayCommand(_mainSystem.ImageScaleFit);
        }
    }
}
