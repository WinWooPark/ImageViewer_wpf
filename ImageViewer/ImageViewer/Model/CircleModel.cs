using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model
{
    public class CircleModel
    {
        private double _x;
        private double _y;
        private double _radius;
        private Brush _fill;

        public CircleModel(double x , double y , double radius)
        {
            X = x; Y = y; _radius = radius;
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value;  }
        }

        public double Radius
        {
            get { return _radius; }
            set { _radius = value;  }
        }

        public Brush Fill
        {
            get { return _fill; }
            set { _fill = value;  }
        }
    }
}
