using ImageView.Model.ManagementSystem;
using ImageView.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageView.API
{
    public class ImageViewAPI
    {
        private static object _obj = new object();
        private static ImageViewAPI _instance;
        public static ImageViewAPI Instance 
        {
            get 
            {
                lock (_obj) 
                {
                    if (_instance == null) 
                    {
                        _instance = new ImageViewAPI();
                    }
                }
                return _instance;
            }
        }
        ////////////////////////////////////////////////////////
        private MainSystem _mainSystem;
        private ImageViewAPI(){}

        public void InitImageView(int ImageWidth , int ImageHeight) 
        {
            _mainSystem = MainSystem.Instance;

            //이미지 사이즈 초기화
            _mainSystem.ImageWidth = ImageWidth;
            _mainSystem.ImageHeight = ImageHeight;
        }


        public void AddDrawObjectEllipse(double X, double Y, double Width, double Height, bool Judge) 
        {
            _mainSystem.AddDrawObjectEllipse(X, Y, Width, Height, Judge);
        }

        public void DrawAllObject() 
        {
            _mainSystem.DrawAllObject();
        }

        public void UpdateImage(BitmapSource Bitmap)
        {
            _mainSystem.DeleteAllDrawObject();
            _mainSystem.UpdateImage(Bitmap);
        }

        public void ImageFit()
        {
            _mainSystem.ImageFit();
        }

        public void DeleteAllDrawObject()
        {
            _mainSystem.DeleteAllDrawObject();
        }
    }
}
