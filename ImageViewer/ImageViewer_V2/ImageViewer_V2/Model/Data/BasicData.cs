using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer_V2.Model.Data
{
    public class BasicData
    {
        string _version = "0.0.2";
        public string Version
        {
            get { return _version; }
            set { if (_version != value) _version = value; }
        }

        int _threshold = 150;
        public int Threshold 
        {
            get { return _threshold; }
            set { if(_threshold != value) _threshold = value; }
        }

        double _reference = 66;
        public double Reference
        {
            get { return _reference; }
            set { if (_reference != value) _reference = value; }
        }

      

        //파일 저장
        public void SaveBasicData() 
        {

        }

        public void LoadBasicData()
        {

        }
    }
}
