using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageViewer_V2.ViewModel;

namespace ImageViewer_V2.View
{
    /// <summary>
    /// ParameterSetting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ParameterSetting : UserControl
    {
        ParameterSettingViewModel _parameterSettingViewModel;
        public ParameterSetting()
        {
            InitializeComponent();
            _parameterSettingViewModel = new ParameterSettingViewModel();
            DataContext = _parameterSettingViewModel;
        }
    }
}
