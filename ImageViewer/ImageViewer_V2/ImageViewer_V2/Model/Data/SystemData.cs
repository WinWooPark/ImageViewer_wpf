using System;
using System.Collections.Concurrent;
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
    }
}
