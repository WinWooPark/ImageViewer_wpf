using ImageViewer_V2.Define;
using ImageViewer_V2.Model.Data;
using Microsoft.Win32;
using OpenCvSharp;
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
                //_integratedClass.ImageViewerViewModel.UpDateMainImage(Bitmap);
                _integratedClass.ImageViewAPI.UpdateImage(Bitmap);
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

        public void ImageScaleFit() 
        {
            _integratedClass.ImageViewAPI.ImageFit();
        }

       

        public void ImageChange()
        {
            
        }

        public void InspctionStart()
        {
            if (_systemData.BlobDatas.Count != 0) _systemData.BlobDatas.Clear();

            //if (_integratedClass.ImageViewerViewModel..BlobDatas.Count != 0) IntegratedClass.Instance.MainViewModel.BlobDatas.Clear();

            if (_systemData.Buffer != null)
                _integratedClass.ImageProcessor.SetImageProcessorBuffer(_systemData.Buffer);
        }

        public void CBcoredoneInspction(double ProcessTime) 
        {
          
        }

       
    }
}
