using ImageViewer_V2.Model.Data;
using ImageViewer_V2.Model.ImageProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace ImageViewer_V2.Model.ManagementSystem
{
    //라이브러리 및 데이터 를 엮는 클래스
    public class IntegratedClass : UiViewModels
    {
        static private IntegratedClass? _instance = null;

        static public IntegratedClass Instance
        {
            get
            {
                if (_instance == null) _instance = new IntegratedClass();

                return _instance;
            }
        }

        BasicData _basicData;
        public BasicData BasicData {get { return _basicData; } }

        SystemData _systemData;
        public SystemData SystemData { get { return _systemData; } }

        ImageProcessor _imageProcessor;
        public ImageProcessor ImageProcessor { get { return _imageProcessor; } }

        private IntegratedClass()
        {
        }

        public void InitIntegratedClass() 
        {
            _basicData = new BasicData();
            _systemData = new SystemData();
            _imageProcessor = new ImageProcessor();
            _imageProcessor.InitImageProcessor(this);
        }

        public void CloseIntegratedClass() 
        {
            
        }
    }
}
