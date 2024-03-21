using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model.MainSystem
{
    class CircleData
    {
        double _radius;

        public double Radius
        {
            get { return _radius; }
            set { if (_radius != value) _radius = value; }
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
    }

    public class BlobData
    {
        int _index;         //Blob 의 인덱스
        bool _result;       //Blob 결과 false = 불양 true = 양품
        int _posGX;         //Blob 의 무게중심점
        int _posGY;         //Blob 의 무게중심점

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

        public int PosGX
        {
            get { return _posGX; }
            set
            {
                if (_posGX != value) { _posGX = value; }
            }
        }

        public int PosGY
        {
            get { return _posGY; }
            set
            {
                if (_posGY != value) { _posGY = value; }
            }
        }
    }
}
