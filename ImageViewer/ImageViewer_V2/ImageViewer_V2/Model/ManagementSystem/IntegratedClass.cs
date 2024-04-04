using ImageViewer_V2.Model.Data;
using ImageViewer_V2.Model.ImageProcess;
using ImageView.API;

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

        ImageViewAPI _imageViewAPI;
        public ImageViewAPI ImageViewAPI { get { return _imageViewAPI; } }

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
            _imageViewAPI = ImageViewAPI.Instance;
            _imageViewAPI.InitImageView(3780,3780);
            _imageProcessor.InitImageProcessor(this);
        }

        public void CloseIntegratedClass() 
        {
            _imageProcessor.CloseImageProcessor();
        }
    }
}
