﻿using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ImageViewer.ViewModel;
using Microsoft.Win32;
using OpenCvSharp;

namespace ImageViewer.Model.MainSystem
{
    public class SystemInfo
    {
        ImageProcessor _instance;
        Mat _image;

        public SystemInfo()
        {
            _instance = ImageProcessor.Instance;
            _instance.InitImageProcessor();
            _instance.SetCallBackFunc(CallBackImage);
        }

        ~SystemInfo() 
        {
            _instance.CloseImageProcessor();
        }

        Action<BitmapSource> _ImageUpdateCallBack;
        public void SetImageUpdateCallBack(Action<BitmapSource> callBack) 
        {
            _ImageUpdateCallBack = callBack;
        }

        public BitmapSource MatToBitmapSource(Mat Image) 
        {
            return OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(Image);
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

        void CallBackImage(Mat Image) 
        {
            _image = Image;
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _ImageUpdateCallBack(MatToBitmapSource(_image));
            });
        }

        public void ImageSave()
        {
            if (_image.Empty()) return;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save Image";
            dlg.DefaultExt = "bmp";

            dlg.FileName = "SaveImage";
            dlg.Filter = "SaveImage (.bmp)|*.BMP";


            if (dlg.ShowDialog() == true)
            {
                string Filename = dlg.FileName;

                Cv2.ImWrite(Filename, _image);
            }
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

        public void StartInspction() 
        {
            if(_image != null)
                _instance.SetImageProcessorBuffer(_image);
        }
    }
}