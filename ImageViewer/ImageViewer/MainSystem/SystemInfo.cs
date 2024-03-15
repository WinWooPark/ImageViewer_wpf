using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer.ViewModel;
using Microsoft.Win32;
using OpenCvSharp;

namespace ImageViewer.MainSystem
{
    public class SystemInfo
    {
        public SystemInfo(){}
        private MainViewModel _mainViewModel;
        Mat _image;
        byte[] _imageBuffer;

        public void SetViewModel(MainViewModel mainViewModel) 
        {
             _mainViewModel = mainViewModel;
        }
        public void ImageRoad() 
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true) 
            {
                _image = Cv2.ImRead(dlg.FileName);
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
            double zoom = _mainViewModel.Scale;
            zoom += 0.1;
            _mainViewModel.Scale = zoom;
        }

        public void ZoomOut() 
        {
            double zoom = _mainViewModel.Scale;
            zoom -= 0.1;
            _mainViewModel.Scale = zoom;
        }
    }
}
