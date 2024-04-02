using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer_V2.Model.Data
{
    public class BasicData
    {
        int _threshold;
        public int Threshold 
        {
            get { return _threshold; }
            set { if(_threshold != value) _threshold = value; }
        }

        double _reference;
        public double Reference
        {
            get { return _reference; }
            set { if (_reference != value) _reference = value; }
        }

        double _processTime;
        public double ProcessTime
        {
            get { return _processTime; }
            set { if (_processTime != value) { _processTime = value; } }
        }

        //파일 저장
        void SaveBasicData() 
        {

        }

        void LoadBasicData()
        {

        }
    }
}
