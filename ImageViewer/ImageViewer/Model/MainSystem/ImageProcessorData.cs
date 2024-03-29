using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ImageViewer.Model.MainSystem
{
    public class BlobData
    {
        int _index;         //Blob 의 인덱스
        bool _result;       //Blob 결과 false = 불양 true = 양품
        double _radius;

        Point2d _centerPoint;
        Point2d _imageControlPoint;
        Point2d _canvasControlPoint;

        Size2d _blobSize;
        Size2d _canVasBlobSize;


        public int Index
        {
            get { return _index; }
            set
            {
                if (_index != value) { _index = value; }
            }
        }

        public bool Result
        {
            get { return _result; }
            set
            {
                if (_result != value) { _result = value; }
            }
        }

        public double Radius
        {
            get { return _radius; }
            set { if (_radius != value) _radius = value; }
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

        public Size2d BlobSize
        {
            get { return _blobSize; }
            set { if (_blobSize != value) _blobSize = value; }
        }
    }
}
