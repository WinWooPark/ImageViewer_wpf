using ImageView.Define;
using System.Windows;

namespace ImageView.Model.ManagementSystem
{
    public class CoordinateTransformations
    {
        MainSystem _mainSystem;
        public CoordinateTransformations(MainSystem mainSystem) 
        {
            _mainSystem = mainSystem;
        }

        public Point CoordinateTransformationsPoint(CommonDefine.Coordinate Mode, Point InputPoint)
        {
            Point OutputPoint = new Point();

            switch (Mode)
            {
                case CommonDefine.Coordinate.eImage2Control:
                    OutputPoint = CoordinateTransformationsImageToControlPoint(InputPoint);
                    break;
                case CommonDefine.Coordinate.eControl2Image:
                    OutputPoint = CoordinateTransformationsControlToImagePoint(InputPoint);
                    break;
            }
            return OutputPoint;
        }

        Point CoordinateTransformationsImageToControlPoint(Point InputPoint)
        {
            Point OutputPoint = new Point();
            //1. Image의 좌표를 Image Control의 좌표로 변환한다.
            double ScaleX = _mainSystem.ImageControlWidth / _mainSystem.ImageWidth;
            double ScaleY = _mainSystem.ImageControlHeight/ _mainSystem.ImageHeight;

            //2. Image Control의 좌표를 Canvas Control의 좌표로 변환한다.
            double ShiftX = (_mainSystem.CanvasControlWidth - _mainSystem.ImageControlWidth) / 2;
            double ShiftY = (_mainSystem.CanvasControlHeight - _mainSystem.ImageControlHeight) / 2;

            OutputPoint.X = InputPoint.X * ScaleX + ShiftX;
            OutputPoint.Y = InputPoint.Y * ScaleY + ShiftY;

            return OutputPoint;
        }

        Point CoordinateTransformationsControlToImagePoint(Point InputPoint)
        {
            Point OutputPoint = new Point();

            //1. Canvas Control의 좌표를 Image Control의 좌표로 변환한다.
            double ShiftX = (_mainSystem.CanvasControlWidth - _mainSystem.ImageControlWidth) / 2;
            double ShiftY = (_mainSystem.CanvasControlHeight - _mainSystem.ImageControlHeight) / 2;

            //2. Image Control의 좌표를 Image의 좌표로 변환한다.
            double ScaleX = _mainSystem.ImageWidth / _mainSystem.ImageControlWidth;
            double ScaleY = _mainSystem.ImageHeight / _mainSystem.ImageControlHeight;

            OutputPoint.X = (InputPoint.X - ShiftX) * ScaleX;
            OutputPoint.Y = (InputPoint.Y - ShiftY) * ScaleY;

            return OutputPoint;
        }

        public Size CoordinateTransformationsLength(CommonDefine.Coordinate Mode, Size Input)
        {
            Size Output = new Size();

            switch (Mode)
            {
                case CommonDefine.Coordinate.eImage2Control:
                    Output = CoordinateTransformationsImageToControlLength(Input);
                    break;
                case CommonDefine.Coordinate.eControl2Image:
                    Output = CoordinateTransformationsControlToImageLength(Input);
                    break;
            }
            return Output;
        }

        Size CoordinateTransformationsImageToControlLength(Size Input)
        {
            Size Output = new Size();
            //1. Image상의 픽셀 거리를  Image Control의 상의 거리로변환한다.
            double ScaleX = (_mainSystem.ImageControlWidth) / _mainSystem.ImageWidth;
            double ScaleY = (_mainSystem.ImageControlHeight) / _mainSystem.ImageWidth;

            Output.Width = Input.Width * ScaleX;
            Output.Height = Input.Height * ScaleY;

            return Output;
        }

        Size CoordinateTransformationsControlToImageLength(Size Input)
        {
            Size Output = new Size();
            //1. Image상의 픽셀 거리를  Image Control의 상의 거리로변환한다.
            double ScaleX = _mainSystem.ImageWidth / _mainSystem.ImageControlWidth;
            double ScaleY = _mainSystem.ImageWidth / _mainSystem.ImageControlHeight;

            Output.Width = Input.Width * ScaleX;
            Output.Height = Input.Height * ScaleY;

            return Output;
        }

    }
}
