using ImageViewer.Model.MainSystem;
using ImageViewer.ViewModel;
using System;
using System.Collections.Concurrent;
using System.Windows.Media;
using OpenCvSharp;

namespace ImageViewer.Model
{
    public class IntegratedClass
    {
        private static IntegratedClass _integratedClass = null;

        public static IntegratedClass Instance 
        {
            get
            {
                if (_integratedClass == null) _integratedClass = new IntegratedClass();

                return _integratedClass;
            }
        }
        private IntegratedClass() 
        {
            _blobDatas = new ConcurrentQueue<BlobData>();
        }

        int _threshold = 150;
        public int Threshold 
        { 
            get { return _threshold; }
            set 
            {
                if(_threshold != value)
                    _threshold = value;
            }
        }

        double _reference = 67;
        public double Reference
        {
            get { return _reference; }
            set
            {
                if (_reference != value)
                    _reference = value;
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
                }
            }
        }

        double _centerPointX;
        public double CenterPointX
        {
            get => _centerPointX;
            set
            {
                if (_centerPointX != value)
                {
                    _centerPointX = value;
                }
            }
        }

        double _centerPointY;
        public double CenterPointY
        {
            get => _centerPointY;
            set
            {
                if (_centerPointY != value)
                {
                    _centerPointY = value;
                }
            }
        }

        double _translateX;
        public double TranslateX
        {
            get { return _translateX; }
            set
            {
                if (_translateX != value)
                {
                    _translateX = value;
                }
            }
        }

        double _translateY;
        public double TranslateY
        {
            get { return _translateY; }
            set
            {
                if (_translateY != value)
                {
                    _translateY = value;
                }
            }
        }

        ConcurrentQueue<BlobData> _blobDatas;
        public void SetBlobData(BlobData blobData) { _blobDatas.Enqueue(blobData); }
        public ConcurrentQueue<BlobData> GetBlobData() { return _blobDatas; }

        ImageSource _mainImage;
        public ImageSource MainImage
        {
            get { return _mainImage; }
            set
            {
                if (_mainImage != value)
                {
                    _mainImage = value;
                }
            }
        }

        ImageSource _subImage;
        public ImageSource SubImage
        {
            get { return _subImage; }
            set
            {
                if (_subImage != value)
                {
                    _subImage = value;
                }
            }
        }

        SystemInfo _systemInfo;
        public SystemInfo SystemInfo 
        {
            get { return _systemInfo; }
            set 
            {
                if(value != _systemInfo)
                    _systemInfo = value;
            }
        }

        MainViewModel _mainViewModel;
        public MainViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set
            {
                if (value != _mainViewModel)
                    _mainViewModel = value;
            }
        }

        public void InspDoneFunc(Mat Image) 
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _mainViewModel.UpdateUI();
                _mainViewModel.UpdateImage(SystemInfo.MatToBitmapSource(Image));
            });
        }

        public void UpdataMainImage(Mat Image)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _mainViewModel.UpdateImage(SystemInfo.MatToBitmapSource(Image));
            });
        }

        public void UpdataSubImage(Mat Image)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _mainViewModel.UpdateSubImage(SystemInfo.MatToBitmapSource(Image));
            });
        }
    }
}
