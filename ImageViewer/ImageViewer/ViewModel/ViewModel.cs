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
using ImageViewer.Behaviors;
using ImageViewer.Model;
using System.Collections.Concurrent;

namespace ImageViewer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private SystemInfo _systeminfo;
      
        public event EventHandler? CanExecuteChanged;

        public MainViewModel() 
        {
            Version = string.Format("Var : {0}", CommonDefine.CommonDefine.Version);

            _comdoItems = new ObservableCollection<string>
            {
                "Original",
                "Filter",
                "Threshold",
                "Bolb"
            };

            BlobData = new ObservableCollection<BlobData>();
            BlobData.Clear();

            _systeminfo = IntegratedClass.Instance.SystemInfo;

            CreateCommands();
        }

        public void UpdateUI() 
        {
            ConcurrentQueue<BlobData> blobDatas = IntegratedClass.Instance.GetBlobData();

            Random rand = new Random();

            foreach (BlobData blobData in blobDatas)
                BlobData.Add(blobData);
        }

        public void UpdateImage(BitmapSource bitmapSource) 
        {
            MainImage = bitmapSource;
        }

        public void UpdateSubImage(BitmapSource bitmapSource)
        {
            SubImage = bitmapSource;
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

        public int Threshold 
        {
            get { return IntegratedClass.Instance.Threshold; }
            set 
            {
                if (IntegratedClass.Instance.Threshold != value)
                {
                    IntegratedClass.Instance.Threshold = value;
                    OnPropertyChanged(nameof(Threshold));
                }
            }
        }

        public double Reference
        {
            get { return IntegratedClass.Instance.Reference; }
            set
            {
                if (IntegratedClass.Instance.Reference != value)
                {
                    IntegratedClass.Instance.Reference = value;
                    OnPropertyChanged(nameof(Reference));
                }
            }
        }

        public ImageSource MainImage 
        {
            get => IntegratedClass.Instance.MainImage;
            set 
            {
                if (IntegratedClass.Instance.MainImage != value)
                {
                    IntegratedClass.Instance.MainImage = value;
                    OnPropertyChanged(nameof(MainImage));
                }
            }
        }

        public ImageSource SubImage
        {
            get => IntegratedClass.Instance.SubImage;
            set
            {
                if (IntegratedClass.Instance.SubImage != value)
                {
                    IntegratedClass.Instance.SubImage = value;
                    OnPropertyChanged(nameof(SubImage));
                }
            }
        }

        
        public double Scale 
        {
            get => IntegratedClass.Instance.Scale;
            set
            {
                if (IntegratedClass.Instance.Scale != value)
                {
                    IntegratedClass.Instance.Scale = value;
                    OnPropertyChanged(nameof(Scale));
                }
            }
        }

        public double CenterPointX
        {
            get => IntegratedClass.Instance.CenterPointX;
            set
            {
                if (IntegratedClass.Instance.CenterPointX != value)
                {
                    IntegratedClass.Instance.CenterPointX = value;
                    OnPropertyChanged(nameof(CenterPointX));
                }
            }
        }

        public double CenterPointY
        {
            get => IntegratedClass.Instance.CenterPointY;
            set
            {
                if (IntegratedClass.Instance.CenterPointY != value)
                {
                    IntegratedClass.Instance.CenterPointY = value;
                    OnPropertyChanged(nameof(CenterPointY));
                }
            }
        }

        public double TranslateX
        {
            get { return IntegratedClass.Instance.TranslateX; }
            set
            { 
                if (IntegratedClass.Instance.TranslateX != value)
                {
                    IntegratedClass.Instance.TranslateX = value;
                    OnPropertyChanged(nameof(TranslateX));
                }
            }
        }

        public double TranslateY
        {
            get { return IntegratedClass.Instance.TranslateY; }
            set
            {
                if (IntegratedClass.Instance.TranslateY != value)
                {
                    IntegratedClass.Instance.TranslateY = value;
                    OnPropertyChanged(nameof(TranslateY));
                }
            }
        }

        private BlobData _selectedBlobItem;
        public BlobData SelectedBlobItem
        {
            get { return _selectedBlobItem; }
            set 
            {
                if (_selectedBlobItem != value)
                {
                    _selectedBlobItem = value;
                    OnPropertyChanged(nameof(_selectedBlobItem));
                }
            }
        }

        private ObservableCollection<BlobData> _blobData;

        public ObservableCollection<BlobData> BlobData
        {
            get { return _blobData; }
            set
            {
                if (_blobData != value) { _blobData = value; }

                OnPropertyChanged(nameof(BlobData));
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

        ICommand _SelectedBlobItemCommand;
        public ICommand SelectedBlobItemCommand { get; set; }

        void CreateCommands() 
        {
            ImageLoad = new CommandsImageLoad(_systeminfo);
            ImageSave = new CommandsImageSave(_systeminfo);
            ImageChange = new CommandsImageChange(_systeminfo);
            Start = new CommandsStart(_systeminfo);
            ImageFit = new CommandsImageFit(_systeminfo);
            ImageZoomIn = new CommandsImageZoomIn(_systeminfo);
            ImageZoomOut = new CommandsImageZoomOut(_systeminfo);
            SelectedBlobItemCommand = new CommandsSelectedBlobItem(_systeminfo);
        }

    }
}
