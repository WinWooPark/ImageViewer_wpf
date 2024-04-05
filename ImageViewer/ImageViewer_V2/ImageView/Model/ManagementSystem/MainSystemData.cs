using ImageView.ViewModel;
using System.Windows;
using System.Drawing;
using Size = System.Windows.Size;

namespace ImageView.Model.ManagementSystem
{
    //여기 안에서 이미지 Zoom Pan Roi 그리기 다 한다.
    //외부에서는 이미지와 ROI 좌표들만 보내준다.

    public class MainSystemData
    {
        public MainSystemData() 
        {
            _drawObj = new DrawObject.DrawObject();
        }

        DrawObject.DrawObject _drawObj;
        public DrawObject.DrawObject DrawObj
        {
            get { return _drawObj; }
            set { _drawObj = value; }
        }

        ImageViewViewModel _imageViewViewModel;
        public ImageViewViewModel ImageViewViewModel 
        {
            get { return _imageViewViewModel; }
            set { _imageViewViewModel = value; }
        }

        int _imageWidth;
        public int ImageWidth
        {
            get { return _imageWidth; }
            set { if (_imageWidth != value) _imageWidth = value; }
        }

        int _imageHeight;
        public int ImageHeight
        {
            get { return _imageHeight; }
            set { if (_imageHeight != value) _imageHeight = value; }
        }

        double _imageControlWidth;
        public double ImageControlWidth
        {
            get { return _imageControlWidth; }
            set { if (_imageControlWidth != value) _imageControlWidth = value; }
        }

        double _imageControlHeight;
        public double ImageControlHeight
        {
            get { return _imageControlHeight; }
            set { if (_imageControlHeight != value) _imageControlHeight = value; }
        }

        double _canvasControlWidth;
        public double CanvasControlWidth
        {
            get { return _canvasControlWidth; }
            set { if (_canvasControlWidth != value) _canvasControlWidth = value; }
        }

        double _canvasControlHeight;
        public double CanvasControlHeight
        {
            get { return _canvasControlHeight; }
            set { if (_canvasControlHeight != value) _canvasControlHeight = value; }
        }

        double _scale = 1;
        public double Scale
        {
            get { return _scale; }
            set { if (_scale != value) _scale = value; }
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

        double _translationX = 0;
        public double TranslationX
        {
            get { return _translationX; }
            set { if (_translationX != value) _translationX = value; }
        }

        double _translationY = 0;
        public double TranslationY
        {
            get { return _translationY; }
            set { if (_translationY != value) _translationY = value; }
        }

        //Size _shift;
        public double ShiftWidth 
        {
            get; 
            set; 
        }
        public double ShiftHeight
        {
            get;
            set;
        }

        //Size _ratio;
        public double RatioX
        {
            get;
            set;
        }
        public double RatioY
        {
            get;
            set;
        }

    }
}
