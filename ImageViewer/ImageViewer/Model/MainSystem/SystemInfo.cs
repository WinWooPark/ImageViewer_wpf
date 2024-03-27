using System;
using System.Collections.Concurrent;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using ImageViewer.ViewModel;
using Microsoft.Win32;
using OpenCvSharp;

namespace ImageViewer.Model.MainSystem
{
    public class SystemInfo
    {
        ImageProcessor _instance;
        Mat _image;

        public Mat Image{ get => _image; }
        public SystemInfo()
        {
            _instance = ImageProcessor.Instance;
            _instance.InitImageProcessor();

            IntegratedClass.Instance.SystemInfo = this;
        }

        ~SystemInfo() 
        {
            
        }

    

        public BitmapSource MatToBitmapSource(Mat Image) 
        {
            return OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(Image);
        }

        public void CloseSystemInfo() 
        {
            _instance.CloseImageProcessor();
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
            }

            if (_image.Empty()) return;

            IntegratedClass.Instance.UpdataMainImage(_image);
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


        public void ZoomIn() 
        {
            
        }

        public void ZoomOut() 
        {

        }

        public void ZoomFit()
        {

            IntegratedClass.Instance.MainViewModel.Scale = 1;
            IntegratedClass.Instance.MainViewModel.TranslateX = 0;
            IntegratedClass.Instance.MainViewModel.TranslateY = 0;
        }

        public void StartInspction() 
        {
            if(IntegratedClass.Instance.GetBlobData().Count != 0) IntegratedClass.Instance.GetBlobData().Clear();

            if (IntegratedClass.Instance.MainViewModel.BlobData.Count != 0) IntegratedClass.Instance.MainViewModel.BlobData.Clear();
            
            if (_image != null)
                _instance.SetImageProcessorBuffer(_image);
        }

        public void ImageChage() 
        {
            string SelectedComdo = IntegratedClass.Instance.MainViewModel.SelectedItem;

           Mat Image = _instance.SelectedImage(SelectedComdo);
        
            if (_image == null || _image.Empty() == true) return;

            IntegratedClass.Instance.UpdataMainImage(Image);
        }

        public void SelectedBlobItem()
        {
            BlobData Selected = IntegratedClass.Instance.MainViewModel.SelectedBlobItem;

            ConcurrentQueue<BlobData> blobDatas = IntegratedClass.Instance.GetBlobData();

            BlobData Same = null;

            foreach (BlobData data in blobDatas) 
            {
                if (data.Index == Selected.Index) 
                {
                    Same = data;
                    break;
                }
            }

            int Margin = 10;

            int StartPointX = (int)(Same.CenterPointX - ((Same.Width + Margin) / 2));
            int StartPointY = (int)(Same.CenterPointY - ((Same.Height + Margin) / 2));


            Rect regionOfInterest = new Rect(StartPointX, StartPointY, (int)Same.Width + Margin, (int)Same.Height + Margin);
            Mat CropImage = new Mat(_instance.Result, regionOfInterest);

            IntegratedClass.Instance.UpdataSubImage(CropImage);
        }
    }
}
