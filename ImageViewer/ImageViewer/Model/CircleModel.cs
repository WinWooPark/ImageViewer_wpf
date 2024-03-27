using System.Drawing;
using ImageViewer.ViewModel;

namespace ImageViewer.Model
{
    public class CircleModel : ViewModel.ViewModelBase
    {
        private int _centerX;
        private int _centerY;
        private int _radius;
        private int _scale;
        private int _translateX;
        private int _translateY;
        private Brush _fill;

        public CircleModel(double centerX , double centerY, double radius, double scale, double translateX, double translateY)
        {
            CircleCenterX = (int)centerX;
            CircleCenterY = (int)centerY;
            CircleRadius = (int)radius;
            CircleScale = (int)scale;
            CircleTranslateX = (int)translateX;
            CircleTranslateY = (int)translateY;
        }

        public int CircleCenterX
        {
            get { return _centerX; }
            set { if(_centerX != value) _centerX = value; OnPropertyChanged(nameof(CircleCenterX)); }
        }

        public int CircleCenterY
        {
            get { return _centerY; }
            set { if (_centerY != value) _centerY = value; OnPropertyChanged(nameof(CircleCenterY)); }
        }

        public int CircleRadius
        {
            get { return _radius; }
            set { if (_radius != value) _radius = value; OnPropertyChanged(nameof(CircleRadius)); }
        }

        public int CircleScale
        {
            get { return _scale; }
            set { if (_scale != value) _scale = value; OnPropertyChanged(nameof(CircleScale)); }
        }

        public int CircleTranslateX
        {
            get { return _translateX; }
            set { if (_translateX != value) _translateX = value; OnPropertyChanged(nameof(CircleTranslateX)); }
        }

        public int CircleTranslateY
        {
            get { return _translateY; }
            set { if (_translateY != value) _translateY = value; OnPropertyChanged(nameof(CircleTranslateY)); }
        }

    }
}
