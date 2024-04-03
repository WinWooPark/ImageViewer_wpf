using System;
using System.Collections.Concurrent;
using ImageViewer_V2.Model.DrawObject;
using System.Windows.Media.Imaging;
using ImageViewer_V2.Model.ImageProcess.Data;
using OpenCvSharp;

namespace ImageViewer_V2.Model.Data
{
    public class SystemData
    {
        public SystemData()
        {
            _blobDatas = new ConcurrentQueue<BlobData> ();
            _reslutEllipse = new ConcurrentQueue<DrawEllipse>();
        }
        static public BitmapSource MatToBitmapSource(Mat Input) 
        {
            BitmapSource bitmapSource = OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(Input);
            return bitmapSource;
        }

        Mat _buffer;
        public Mat Buffer
        {
            get { return _buffer; }
            set { if (_buffer != value) _buffer = value; }
        }

        double _imagecontrolWidth;
        public double ImagecontrolWidth
        {
            get { return _imagecontrolWidth; }
            set { if (_imagecontrolWidth != value) _imagecontrolWidth = value; }
        }

        double _imagecontrolHeight;
        public double ImagecontrolHeight
        {
            get { return _imagecontrolHeight; }
            set { if (_imagecontrolHeight != value) _imagecontrolHeight = value; }
        }

        double _canvasControlWidth;
        public double CanvasControlWidth
        {
            get { return _canvasControlWidth; }
            set { if (_canvasControlWidth != value) _canvasControlWidth = value; }
        }

        double _canvasControlHeight;
        public double CanvasControlHeight
        {
            get { return _canvasControlHeight; }
            set { if (_canvasControlHeight != value) _canvasControlHeight = value; }
        }

        double _scale = 1;
        public double Scale
        {
            get { return _scale; }
            set { if (_scale != value) _scale = value; }
        }

        double _centerPointX;
        public double CenterPointX
        {
            get { return _centerPointX; }
            set { if (_centerPointX != value) _centerPointX = value; }
        }

        double _centerPointY;
        public double CenterPointY
        {
            get { return _centerPointY; }
            set { if (_centerPointY != value) _centerPointY = value; }
        }


        double _translationX = 0;
        public double TranslationX
        {
            get { return _translationX; }
            set { if (_translationX != value) _translationX = value; }
        }

        double _translationY = 0;
        public double TranslationY
        {
            get { return _translationY; }
            set { if (_translationY != value) _translationY = value; }
        }

        double _processTime;
        public double ProcessTime
        {
            get { return _processTime; }
            set { if (_processTime != value) { _processTime = value; } }
        }

        ConcurrentQueue<BlobData> _blobDatas;
        public ConcurrentQueue<BlobData> BlobDatas 
        {
            get { return _blobDatas; }
            set { if (_blobDatas != value) _blobDatas = value; }
        }

        ConcurrentQueue<DrawEllipse> _reslutEllipse;
        public ConcurrentQueue<DrawEllipse> ReslutEllipse 
        {
            get { return _reslutEllipse; }
            set { if (_reslutEllipse != value) _reslutEllipse = value; }
        }


    }
}
