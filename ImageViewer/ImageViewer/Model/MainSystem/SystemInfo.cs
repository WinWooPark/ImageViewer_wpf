using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ImageViewer.ViewModel;
using Microsoft.Win32;
using OpenCvSharp;

namespace ImageViewer.Model.MainSystem
{
    public class SystemInfo
    {
        public SystemInfo(){}
        Mat _image;

        Action<BitmapSource> _ImageUpdateCallBack;
        public void SetImageUpdateCallBack(Action<BitmapSource> callBack) 
        {
            _ImageUpdateCallBack = callBack;
        }

        public BitmapSource MatToBitmapSource(Mat Image) 
        {
            return OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(Image);
        }

        public BitmapSource MatToBitmapSource()
        {
            return OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(_image);
        }

        public void ImageRoad() 
        {
            //시스템 내부에서는 Mat으로 가지고 다니다가 Image를 띄울때만 바꿔서 띄운다.
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "*.bmp | *.BMP";
        
            if (dlg.ShowDialog() == true) 
            {
                _image = Cv2.ImRead(dlg.FileName);

                _ImageUpdateCallBack(MatToBitmapSource(_image));
            }
        }

        public void ImageSave(string Path)
        {
            if (_image.Empty()) return;

            Cv2.ImWrite(Path, _image);
        }

        public void UpdateImage() 
        {

        }

        public void ZoomIn() 
        {
            
        }

        public void ZoomOut() 
        {

        }
    }
}
