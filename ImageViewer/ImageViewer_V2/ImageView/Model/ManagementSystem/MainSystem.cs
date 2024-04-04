using ImageView.Define;
using ImageView.Model.DrawObject;
using ImageView.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ImageView.Model.ManagementSystem
{
    public class MainSystem : MainSystemData
    {
        private static object obj = new object();
        private static MainSystem instance;
        public static MainSystem Instance 
        {
            get 
            {
                lock (obj) 
                {
                    if (instance == null)
                    {
                        instance = new MainSystem();
                    }
                }
                return instance;
            }
        }

        CoordinateTransformations _coordinateTransformations;

        public MainSystem()
        {
            _coordinateTransformations = new CoordinateTransformations(this);
        }

        public void GetImageControlSize(double Width, double Height) 
        {
            ImageControlWidth = Width;
            ImageControlHeight = Height;
        }

        public void GetCanvasControlSize(double Width, double Height)
        {
            CanvasControlWidth = Width;
            CanvasControlHeight = Height;
        }

        public void ImageScaleChange(int Delta, double Width, double Height)
        {
            ImageControlWidth = Width;
            ImageControlHeight = Height;

            if (Delta > 0)
            {
                ImageViewViewModel.Scale += CommonDefine.ScaleStep;
                if (ImageViewViewModel.Scale > CommonDefine.ScaleMax) ImageViewViewModel.Scale = CommonDefine.ScaleMax;
            }
            else
            {
                ImageViewViewModel.Scale -= CommonDefine.ScaleStep;
                if (ImageViewViewModel.Scale < CommonDefine.ScaleMin) ImageViewViewModel.Scale = CommonDefine.ScaleMin;
            }

            ImageViewViewModel.CenterPointX = Width / 2;
            ImageViewViewModel.CenterPointY = Height / 2;
        }

        public void ImageTranslationChange(double offsetX, double offsetY)
        {
            //스케일을 곱해주는 이유는 확대 되었을때 그만큼 더 움직일수 있도록 가중치를 부여 한것이다.
            ImageViewViewModel.TranslationX += (offsetX * Scale);
            ImageViewViewModel.TranslationY += (offsetY * Scale);
        }

        public void GetDrawObjectEllipse(double X , double Y , double Width , double Height , bool Judge)
        {
           Point point = new Point();
           Size size = new Size();
           

           point = _coordinateTransformations.CoordinateTransformationsPoint(CommonDefine.Coordinate.eImage2Control, new System.Windows.Point(X, Y));
           size = _coordinateTransformations.CoordinateTransformationsLength(CommonDefine.Coordinate.eImage2Control, new System.Windows.Size(Width, Height));

            DrawEllipse ellipse = new DrawEllipse(point, size);

            ellipse.Fill = Brushes.Green;
            
            if (Judge == false)
                ellipse.Fill = Brushes.Red;

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                ImageViewViewModel.DrawEllipses.Add(ellipse);
            });

        }
    }
}
