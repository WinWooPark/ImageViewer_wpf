using System.Windows.Media;
using ImageViewer.ViewModel;
using OpenCvSharp;

namespace ImageViewer.Model
{
    public class DrawEllipse
    {
        Point2d _centerPoint;
        Size2d _blobSize;
        Brush _fill;

        public Brush Fill 
        {
            get { return _fill; }
            set { if (_fill != value) _fill = value; }
        }

        public double CenterPointX
        {
            get { return _centerPoint.X; }
            set { if (_centerPoint.X != value) _centerPoint.X = value; }
        }

        public double CenterPointY
        {
            get { return _centerPoint.Y; }
            set { if (_centerPoint.Y != value) _centerPoint.Y = value; }
        }

        public Point2d CenterPoint
        {
            get { return _centerPoint; }
            set { if (_centerPoint != value) _centerPoint = value; }
        }

        public double BlobWidth
        {
            get { return _blobSize.Width; }
            set { if (_blobSize.Width != value) _blobSize.Width = value; }
        }

        public double BlobHeight
        {
            get { return _blobSize.Height; }
            set { if (_blobSize.Height != value) _blobSize.Height = value; }
        }

        public Size2d BlobSize
        {
            get { return _blobSize; }
            set { if (_blobSize != value) _blobSize = value; }
        }
    }
}
