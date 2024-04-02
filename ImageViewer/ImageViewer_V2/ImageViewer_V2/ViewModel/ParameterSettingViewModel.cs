using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageViewer_V2.Model.Data;
using ImageViewer_V2.Model.MainSystem;

namespace ImageViewer_V2.ViewModel
{
    public class ParameterSettingViewModel : ObservableObject
    {
        IntegratedClass _integratedClass;
        BasicData _basicData;

        public ParameterSettingViewModel()
        {
            _integratedClass = IntegratedClass.Instance;
            _basicData = _integratedClass.BasicData;
        }

        public int Threshold 
        {
            get  { return _basicData.Threshold; }
            set 
            {
                if (_basicData.Threshold != value) 
                {
                    _basicData.Threshold = value;
                    OnPropertyChanged(nameof(Threshold));
                }
            }
        }
    }
}
