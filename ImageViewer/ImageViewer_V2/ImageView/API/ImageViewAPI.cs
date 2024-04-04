using ImageView.Model.ManagementSystem;
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


        public void GetDrawObjectEllipse(double X, double Y, double Width, double Height, bool Judge) 
        {
            _mainSystem.GetDrawObjectEllipse(X, Y, Width, Height, Judge);
        }

       public void UpdateImage(BitmapSource Bitmap) 
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _mainSystem.ImageViewViewModel.MainImage = Bitmap;
                _mainSystem.ImageViewViewModel.UpdateResult();
            });
       }

       public void ImageFit() 
       {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _mainSystem.ImageViewViewModel.Scale = 1;
                _mainSystem.ImageViewViewModel.TranslationX = 0;
                _mainSystem.ImageViewViewModel.TranslationY = 0;
            });
       }

    }
}
