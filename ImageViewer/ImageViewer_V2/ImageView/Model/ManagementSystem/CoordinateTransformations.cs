using ImageView.Define;
using System.Windows;

namespace ImageView.Model.ManagementSystem
{
    internal class CoordinateTransformations
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
            
            OutputPoint.X = InputPoint.X * _mainSystem.RatioX;
            OutputPoint.Y = InputPoint.Y * _mainSystem.RatioY;

            return OutputPoint;
        }

        Point CoordinateTransformationsControlToImagePoint(Point InputPoint)
        {
            Point OutputPoint = new Point();

            OutputPoint.X = InputPoint.X /_mainSystem.RatioX;
            OutputPoint.Y = InputPoint.Y /_mainSystem.RatioY;

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
            
            Output.Width = Input.Width * _mainSystem.RatioX;
            Output.Height = Input.Height * _mainSystem.RatioY;

            return Output;
        }

        Size CoordinateTransformationsControlToImageLength(Size Input)
        {
            Size Output = new Size();
     
            Output.Width = Input.Width / _mainSystem.RatioX;
            Output.Height = Input.Height / _mainSystem.RatioY;

            return Output;
        }

    }
}
