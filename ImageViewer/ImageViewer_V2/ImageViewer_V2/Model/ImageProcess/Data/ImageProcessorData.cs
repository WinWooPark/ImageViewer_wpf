using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer_V2.Model.ImageProcess.Data
{
    public class BlobData
    {
        //BlobData의 기준은 모두 이미지의 픽셀 좌표.

        int _index;         //Blob 의 인덱스
        bool _result;       //Blob 결과 false = 불양 true = 양품
        double _radius;     //Blob 반지름

        Point2d _centerPoint; //Blob의 중점
        Point2d _leftTopPoint; //Blob의 좌상단점
        Size2d _blobSize; //Blob 사이즈
       
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

        public Point2d LeftTopPoint
        {
            get { return _leftTopPoint; }
            set { if (_leftTopPoint != value) _leftTopPoint = value; }
        }

        public Size2d BlobSize
        {
            get { return _blobSize; }
            set { if (_blobSize != value) _blobSize = value; }
        }

        public void CalBlobSize() 
        {
            this._blobSize = new Size2d(this._radius * 2, this._radius * 2);
            this.LeftTopPoint = new Point2d(this._centerPoint.X - this._radius, this._centerPoint.Y - this._radius);
        }
    }
}
