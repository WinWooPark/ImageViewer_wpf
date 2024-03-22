using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model.MainSystem
{
    public class BlobData
    {
        int _index;         //Blob 의 인덱스
        bool _result;       //Blob 결과 false = 불양 true = 양품
        double _radius;
        double _centerPointX;
        double _centerPointY;
        double _width;
        double _height;

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
            get { return _centerPointX; }
            set { if (_centerPointX != value) _centerPointX = value; }
        }
       
        public double CenterPointY
        {
            get { return _centerPointY; }
            set { if (_centerPointY != value) _centerPointY = value; }
        }

        public double Width
        {
            get { return _width; }
            set { if (_width != value) _width = value; }
        }

        public double Height
        {
            get { return _height; }
            set { if (_height != value) _height = value; }
        }

    }
}
