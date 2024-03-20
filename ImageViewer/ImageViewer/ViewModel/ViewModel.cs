using System;
using System.Windows.Input;
using ImageViewer.Model.MainSystem;
using ImageViewer.Command;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Collections.ObjectModel;



namespace ImageViewer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private SystemInfo _systeminfo;
      
        public event EventHandler? CanExecuteChanged;

        public MainViewModel()
        {
             
        }
        public MainViewModel(SystemInfo systemInfo) 
        {
            _systeminfo = systemInfo;
            _systeminfo.SetImageUpdateCallBack(UpdateImage);

            Version = string.Format("Var : {0}", CommonDefine.CommonDefine.Version);


            _comdoItems = new ObservableCollection<string>
            {
                "Original",
                "Gaussian Filter",
                "Threshold",
                "Bolb"
            };

            CreateCommands();
        }

        void UpdateImage(BitmapSource bitmapSource) 
        {
            MainImage = bitmapSource;
        }

        string _version;
        public string Version 
        {
            get { return _version; }
            set 
            {
                if (_version != value) 
                {
                    _version = value;
                    OnPropertyChanged(nameof(Version));
                }
            }
        }

        int _threshold;
        public int Threshold 
        {
            get { return _threshold; }
            set 
            {
                if (_threshold != value)
                {
                    _threshold = value;
                    OnPropertyChanged(nameof(Threshold));
                }
            }
        }

        int _mainWidth;
        public int MainWidth { get { return _mainWidth; }}

        int _mainHeight;
        public int MainHeight { get { return _mainHeight; } }

        ImageSource _mainImage;
        public ImageSource MainImage 
        {
            get => _mainImage;
            set 
            {
                if (_mainImage != value)
                {
                    _mainImage = value;
                    OnPropertyChanged(nameof(MainImage));
                }
            }
        }

        ImageSource _subImage;
        public ImageSource SubImage
        {
            get => _subImage;
            set
            {
                if (_subImage != value)
                {
                    _subImage = value;
                    OnPropertyChanged(nameof(SubImage));
                }
            }
        }

        double _scale = 1.0;
        public double Scale 
        {
            get => _scale;
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    OnPropertyChanged(nameof(Scale));
                }
            }
        }

        private ObservableCollection<string> _comdoItems;
        public ObservableCollection<string> ComdoItems
        {
            get { return _comdoItems; }
            set
            {
                if (_comdoItems != value)
                {
                    _comdoItems = value;
                    OnPropertyChanged(nameof(ComdoItems));
                }
            }
                
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        ICommand _ImageLoad;
        public ICommand ImageLoad { get; set; }

        ICommand _ImageSave;
        public ICommand ImageSave { get; set; }

        ICommand _ImageChange;
        public ICommand ImageChange { get; set; }

        ICommand _Start;
        public ICommand Start { get; set; }

        ICommand _ImageFit;
        public ICommand ImageFit { get; set; }

        ICommand _ImageZoomIn; 
        public ICommand ImageZoomIn { get; set; }

        ICommand _ImageZoomOut;
        public ICommand ImageZoomOut { get; set; }

        void CreateCommands() 
        {
            ImageLoad = new CommandsImageLoad(_systeminfo);
            ImageSave = new CommandsImageSave(_systeminfo);
            ImageChange = new CommandsImageChange(_systeminfo);
            Start = new CommandsStart(_systeminfo);
            ImageFit = new CommandsImageFit(_systeminfo);
            ImageZoomIn = new CommandsImageZoomIn(_systeminfo);
            ImageZoomOut = new CommandsImageZoomOut(_systeminfo);
        }

    }
}
