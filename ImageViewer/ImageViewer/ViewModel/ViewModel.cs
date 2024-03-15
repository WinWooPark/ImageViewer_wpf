using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using ImageViewer.MainSystem;
using ImageViewer.Command;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;

using System.IO;
using System.Windows.Media.Media3D;
using System.Drawing.Imaging;
using System.Windows.Controls;
using OpenCvSharp;
using DevExpress.Utils.Svg;

namespace ImageViewer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private SystemInfo _systeminfo;
      
        public event EventHandler? CanExecuteChanged;

        public MainViewModel(SystemInfo systemInfo) 
        {
            _systeminfo = systemInfo;
             Version = CommonDefine.CommonDefine.Version;

            string imagePath = "E:\\5. Project_Test\\3. WPF_ImageViewer\\ImageViewer\\8666420.jpg"; // 이미지 파일 경로 지정
            // BitmapSource를 만듭니다.

            MainImage = Imgload(imagePath);
            SubImage = Imgload(imagePath);
            CreateCommands();
        }

        private BitmapImage Imgload(string strpath)
        {
            try
            {
                FileInfo fio = new FileInfo(strpath);
                if (fio.Exists)
                {
                    BitmapImage img = new BitmapImage();

                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    
                    img.UriSource = new Uri(strpath, UriKind.RelativeOrAbsolute);
                    img.EndInit();

                    if (img.CanFreeze) img.Freeze();

                    return img;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
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

        BitmapImage _mainImage;
        public BitmapImage MainImage 
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

        private void UpdateCenterPoint()
        {
            CenterPoint = new System.Drawing.Point((int)(_mainImage.PixelHeight* Scale / 2.0), (int)(_mainImage.PixelWidth * Scale / 2.0));
        }

        private System.Drawing.Point _centerPoint;
        public System.Drawing.Point CenterPoint
        {
            get { return _centerPoint; }
            set
            {
                _centerPoint = value;
                OnPropertyChanged(nameof(CenterPoint));
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
                    UpdateCenterPoint();
                    OnPropertyChanged(nameof(Scale));
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
