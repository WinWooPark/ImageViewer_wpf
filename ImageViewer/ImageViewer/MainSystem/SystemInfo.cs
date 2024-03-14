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
    class SystemInfo
    {
        Mat _image;
        byte[] _imageBuffer;

        MainViewModel _mainViewModel;

        public static void ImageRoad() 
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true) 
            {
                Mat Image = Cv2.ImRead(dlg.FileName);

                
            }
        }

        public void GetViewModel(MainViewModel mainViewModel) 
        {
            _mainViewModel = mainViewModel;
        }

        public void TESTViewmodel() 
        {
            int a = _mainViewModel.Threshold;
        }
    }
}
