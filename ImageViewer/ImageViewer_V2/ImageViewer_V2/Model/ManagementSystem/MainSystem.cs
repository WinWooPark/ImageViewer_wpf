using ImageViewer_V2.Define;
using ImageViewer_V2.Model.Data;
using ImageViewer_V2.Model.DrawObject;
using ImageViewer_V2.Model.ImageProcess.Data;
using ImageViewer_V2.ViewModel;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageViewer_V2.Model.ManagementSystem
{
    //기능 들이 들어 있는 Class
    public class MainSystem
    {
        static MainSystem? _instance = null;
        public static MainSystem Instance 
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new MainSystem();
                }
                
                return _instance;
            }
        }

        IntegratedClass _integratedClass;
        public IntegratedClass IntegratedClass {get { return _integratedClass; }}

        BasicData _basicData;
        SystemData _systemData;

        private MainSystem() 
        {
            
        }

        public void InitMainSystem() 
        {
            _integratedClass = IntegratedClass.Instance;
            _integratedClass.InitIntegratedClass();
            _basicData = _integratedClass.BasicData;
            _systemData = _integratedClass.SystemData;
        }

        public void ExitProgram() 
        {
            //여기에서 프로그램 종료 루틴을 탄다.

            //라이브러리 종료
            _integratedClass.CloseIntegratedClass();

            App.Current.Shutdown();
        }

        public void ImageLoad() 
        {
            //시스템 내부에서는 Mat으로 가지고 다니다가 Image를 띄울때만 바꿔서 띄운다.
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "*.bmp | *.BMP";

            if (dlg.ShowDialog() == true)
            {
                _systemData.Buffer = Cv2.ImRead(dlg.FileName);
                BitmapSource Bitmap = SystemData.MatToBitmapSource(_systemData.Buffer);
                _integratedClass.ImageViewerViewModel.UpDateMainImage(Bitmap);
            }
        }

        public void ImageSave()
        {
            if (_systemData.Buffer.Empty()) return;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save Image";
            dlg.DefaultExt = CommonDefine.BMP;

            dlg.FileName = "SaveImage";
            dlg.Filter = "SaveImage (.bmp)|*.BMP";


            if (dlg.ShowDialog() == true)
            {
                string Filename = dlg.FileName;

                Cv2.ImWrite(Filename, _systemData.Buffer);
            }
        }

        public void ImageScaleChange(int Delta, double Width, double Height) 
        {
            _systemData.ImagecontrolWidth = Width;
            _systemData.ImagecontrolHeight = Height;

            if (Delta > 0)
            {
                _integratedClass.ImageViewerViewModel.Scale += CommonDefine.ScaleStep;
                if (_systemData.Scale > CommonDefine.ScaleMax) _integratedClass.ImageViewerViewModel.Scale = 8;
            }
            else
            {
                _integratedClass.ImageViewerViewModel.Scale -= CommonDefine.ScaleStep;
                if (_integratedClass.ImageViewerViewModel.Scale < CommonDefine.ScaleMin) _integratedClass.ImageViewerViewModel.Scale = 0.9;
            }

            _integratedClass.ImageViewerViewModel.CenterPointX = Width / 2;
            _integratedClass.ImageViewerViewModel.CenterPointY = Height / 2;
            ReDrawObject();
        }

        public void ImageTranslationChange(double offsetX, double offsetY)
        {
            _integratedClass.ImageViewerViewModel.TranslationX += offsetX;
            _integratedClass.ImageViewerViewModel.TranslationY += offsetY;
            ReDrawObject();
        }

        public void ImageScaleFit() 
        {
            _integratedClass.ImageViewerViewModel.Scale = 1;
            _integratedClass.ImageViewerViewModel.TranslationX = 1;
            _integratedClass.ImageViewerViewModel.TranslationY = 1;
            ReDrawObject();
        }

        public void GetImageContolSize(double Width , double Height)
        {
            _systemData.ImagecontrolWidth = Width;
            _systemData.ImagecontrolHeight = Height;
        }

        public void GetCanvasContolSize(double Width, double Height)
        {
            _systemData.CanvasControlWidth= Width;
            _systemData.CanvasControlHeight = Height;
        }

        void ReDrawObject() 
        {
            _systemData.ReslutEllipse.Clear();

            foreach (BlobData blobData in _systemData.BlobDatas) 
            {
                DrawEllipse drawEllipse = new DrawEllipse();

                Point2d point = ImageToImageControlCoordi(blobData.CenterPoint);
                drawEllipse.CenterPoint = ImageControlToCanvasControlCoordi(point, blobData.BlobSize);

                _systemData.ReslutEllipse.Enqueue(drawEllipse);
            }

            _integratedClass.ImageViewerViewModel.DrawResult();
        }
        public void ImageChange()
        {
            
        }

        public void InspctionStart()
        {
            if (_systemData.BlobDatas.Count != 0) _systemData.BlobDatas.Clear();

            //if (_integratedClass.ImageViewerViewModel..BlobDatas.Count != 0) IntegratedClass.Instance.MainViewModel.BlobDatas.Clear();


            if (_systemData.ReslutEllipse.Count != 0) _systemData.ReslutEllipse.Clear();

            if (_integratedClass.ImageViewerViewModel.DrawEllipses.Count != 0) _integratedClass.ImageViewerViewModel.DrawEllipses.Clear();

            if (_systemData.Buffer != null)
                _integratedClass.ImageProcessor.SetImageProcessorBuffer(_systemData.Buffer);
        }

        public void CBcoredoneInspction(Mat Image) 
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _integratedClass.ImageViewerViewModel.UpDateMainImage(SystemData.MatToBitmapSource(Image));
                _integratedClass.ImageViewerViewModel.DrawResult();
            });
        }

        public Point2d ImageToImageControlCoordi(Point2d InputPoint)
        {
            //Image 좌표에서 Image Control 좌표로 는 스케일 변환
            Point2d OutputPoint = new Point2d();

            double ScaleX = (_systemData.ImagecontrolWidth * _systemData.Scale) / _systemData.Buffer.Width;
            double ScaleY = (_systemData.ImagecontrolHeight * _systemData.Scale) / _systemData.Buffer.Height;

            OutputPoint.X = InputPoint.X * ScaleX;
            OutputPoint.Y = InputPoint.Y * ScaleY;

            return OutputPoint;
        }

        public Point2d ImageControlToCanvasControlCoordi(Point2d InputPoint, Size2d InputLength)
        {
            //Image Control  Canvas로 좌표 변화는 이동변환
            Point2d OutputPoint = new Point2d();

            //캔버스는 좌상단 시작 , 이미지는 중앙 정렬
            double shiftX = ((_systemData.CanvasControlWidth - _systemData.ImagecontrolWidth) / 2) + _systemData.TranslationX;
            double shiftY = ((_systemData.CanvasControlHeight - _systemData.ImagecontrolHeight) / 2) + _systemData.TranslationY;

            OutputPoint.X = InputPoint.X + shiftX - (InputLength.Width / 2);
            OutputPoint.Y = InputPoint.Y + shiftY - (InputLength.Height / 2);

            return OutputPoint;
        }

        public Size2d ImageToImageControlLength(Size2d InputLength)
        {
            Size2d Length = new Size2d();

            double ScaleX = (_systemData.ImagecontrolWidth / _systemData.Scale) / _systemData.Buffer.Width;
            double ScaleY = (_systemData.ImagecontrolHeight / _systemData.Scale) / _systemData.Buffer.Height;

            //반지름이기 때문에 2를 곱한다.
            Length.Width = InputLength.Width * ScaleX * 2;
            Length.Height = InputLength.Height * ScaleY * 2;

            return Length;
        }

        //public Point2d ImageToControlCoordinateTransformations(CommonDefine.Coordinate mode)
        //{
        //    Point2d OutputPoint = new Point2d();

        //    switch (mode)
        //    {
        //        case CommonDefine.Coordinate.eImage2Control:



        //            break;
        //        case CommonDefine.Coordinate.eControl2Image:
        //            break;
        //    }

        //    return OutputPoint;
        //}
    }
}
