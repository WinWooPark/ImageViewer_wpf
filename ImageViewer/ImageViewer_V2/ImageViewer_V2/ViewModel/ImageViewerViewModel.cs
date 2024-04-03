using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageViewer_V2.Model.Data;
using ImageViewer_V2.Model.DrawObject;
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
            _drawEllipse = new ObservableCollection<DrawEllipse>();
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

        private ObservableCollection<DrawEllipse> _drawEllipse;
        public ObservableCollection<DrawEllipse> DrawEllipses
        {
            get { return _drawEllipse; }
            set
            {
                if (_drawEllipse != value)
                {
                    _drawEllipse = value;
                    OnPropertyChanged(nameof(DrawEllipses));
                }
            }
        }

        public void DrawResult()
        {
            ConcurrentQueue<DrawEllipse> drawEllipse = _systemData.ReslutEllipse;

            foreach (DrawEllipse Ellipse in drawEllipse)
                DrawEllipses.Add(Ellipse);
        }
        
        public double Scale
        {
            get { return _systemData.Scale; }
            set 
            {
                if (_systemData.Scale != value)
                {
                    _systemData.Scale = value;
                    OnPropertyChanged(nameof(Scale));
                }
            }
        }

        public double CenterPointX
        {
            get { return _systemData.CenterPointX; }
            set
            {
                if (_systemData.CenterPointX != value)
                {
                    _systemData.CenterPointX = value;
                    OnPropertyChanged(nameof(CenterPointX));
                }
            }
        }

        public double CenterPointY
        {
            get { return _systemData.CenterPointY; }
            set
            {
                if (_systemData.CenterPointY != value)
                {
                    _systemData.CenterPointY = value;
                    OnPropertyChanged(nameof(CenterPointY));
                }
            }
        }


        public double TranslationX
        {
            get { return _systemData.TranslationX; }
            set
            {
                if (_systemData.TranslationX != value)
                {
                    _systemData.TranslationX = value;
                    OnPropertyChanged(nameof(TranslationX));
                }
            }
        }

        public double TranslationY
        {
            get { return _systemData.TranslationY; }
            set
            {
                if (_systemData.TranslationY != value)
                {
                    _systemData.TranslationY = value;
                    OnPropertyChanged(nameof(TranslationY));
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


        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ImageFitCommand { get; set; }
        void Createcommand() 
        {
            ExitCommand = new RelayCommand(_mainSystem.ExitProgram);
            ImageFitCommand = new RelayCommand(_mainSystem.ImageScaleFit);
        }
    }
}
