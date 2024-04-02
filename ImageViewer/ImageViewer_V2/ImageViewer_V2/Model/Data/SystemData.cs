using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageViewer_V2.Model.Data
{
    public class SystemData
    {
        double _scale;
        public double Scale
        {
            get { return _scale; }
            set { if (_scale != value) _scale = value; }
        }

    }
}
