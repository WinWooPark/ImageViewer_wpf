
using System.Windows;
using System.Windows.Media;

namespace ImageView.Model.DrawObject
{
    public class DrawEllipse
    {
        public DrawEllipse(){}

        public DrawEllipse(Point point , Size size)
        {
            _centerPoint = point;
            _ellipseSize = size;
        }

        SolidColorBrush _fill;
        public SolidColorBrush Fill 
        {
            get { return _fill; }
            set { _fill = value; }
        }

        Point _centerPoint;
        public Point CenterPoint 
        {
            get { return _centerPoint; }
            set { _centerPoint = value; }
        }

        Size _ellipseSize;

        public Size EllipseSize 
        {
            get { return _ellipseSize; }
            set { _ellipseSize = value; }
        }

        public double CenterPointX 
        {
            get { return _centerPoint.X; }
            set { _centerPoint.X = value; }
        }

        public double CenterPointY
        {
            get { return _centerPoint.Y; }
            set { _centerPoint.Y = value; }
        }

        public double EllipseWidth
        {
            get { return _ellipseSize.Width; }
            set { _ellipseSize.Width = value; }
        }

        public double EllipseHight
        {
            get { return _ellipseSize.Height; }
            set { _ellipseSize.Height = value; }
        }
    }
}
