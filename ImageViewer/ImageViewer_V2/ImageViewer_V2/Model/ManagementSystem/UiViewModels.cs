using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer_V2.ViewModel;

namespace ImageViewer_V2.Model.ManagementSystem
{
    public abstract class UiViewModels
    {
        MainViewModel _mainViewModel;
        public MainViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set { if (_mainViewModel != value) _mainViewModel = value; }
        }

        MainFrameViewModel _mainFrameViewModel;
        public MainFrameViewModel MainFrameViewModel
        {
            get { return _mainFrameViewModel; }
            set { if (_mainFrameViewModel != value) _mainFrameViewModel = value; }
        }

        ImageLoadViewModel _imageLoadViewModel;
        public ImageLoadViewModel ImageLoadViewModel 
        {
            get { return _imageLoadViewModel; }
            set { if(_imageLoadViewModel != value) _imageLoadViewModel = value; }
        }

        ParameterSettingViewModel _parameterSettingViewModel;
        public ParameterSettingViewModel ParameterSettingViewModel
        {
            get { return _parameterSettingViewModel; }
            set { if (_parameterSettingViewModel != value) _parameterSettingViewModel = value; }
        }

        ImageViewerViewModel _imageViewerViewModel;
        public ImageViewerViewModel ImageViewerViewModel
        {
            get { return _imageViewerViewModel; }
            set { if (_imageViewerViewModel != value) _imageViewerViewModel = value; }
        }
    }
}
