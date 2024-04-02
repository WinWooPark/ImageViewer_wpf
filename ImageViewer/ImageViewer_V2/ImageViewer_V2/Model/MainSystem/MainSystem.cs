using ImageViewer_V2.ViewModel;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer_V2.Model.MainSystem
{
    //기능 들이 들어 있는 Class
    public class MainSystem
    {
        static MainSystem _instance;
        public static MainSystem Instance 
        {
            get 
            {
                if(_instance == null) _instance = new MainSystem();

                return _instance;
            }
        }

        IntegratedClass _integratedClass;

        Dictionary<string, object> _viewModels;
        public Dictionary<string, object> ViewModels 
        {
            get { return _viewModels; }
            set { _viewModels = value; }
        }

        Mat _buffer;

        private MainSystem() 
        {
            InitMainSystem();
        }

        void InitMainSystem() 
        {
            _integratedClass = IntegratedClass.Instance;
            _viewModels = new Dictionary<string, object>();
        }

        public void ExitProgram() 
        {
            //여기에서 프로그램 종료 루틴을 탄다.
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

                _buffer = Cv2.ImRead(dlg.FileName);

                object obj = _viewModels["ImageViewerViewModel"];
                ImageViewerViewModel model = obj as ImageViewerViewModel;

                model.UpDateMainImage(OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(_buffer));
            }
        }

        public void ImageSave()
        {
            if (_buffer.Empty()) return;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save Image";
            dlg.DefaultExt = "bmp";

            dlg.FileName = "SaveImage";
            dlg.Filter = "SaveImage (.bmp)|*.BMP";


            if (dlg.ShowDialog() == true)
            {
                string Filename = dlg.FileName;

                Cv2.ImWrite(Filename, _buffer);
            }
        }

        public void ImageChange()
        {
            
        }

        public void InspctionStart()
        {

        }
    }
}
