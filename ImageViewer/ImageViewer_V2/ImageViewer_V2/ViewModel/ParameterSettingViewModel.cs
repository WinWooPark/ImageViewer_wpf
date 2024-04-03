using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageViewer_V2.Model.Data;
using ImageViewer_V2.Model.ManagementSystem;

namespace ImageViewer_V2.ViewModel
{
    public class ParameterSettingViewModel : ObservableObject
    {
        IntegratedClass _integratedClass;
        BasicData _basicData;
        SystemData _systemData;
        MainSystem _mainSystem;

        public ParameterSettingViewModel()
        {
            _mainSystem = MainSystem.Instance;
            _integratedClass = _mainSystem.IntegratedClass;
            _integratedClass.ParameterSettingViewModel = this;
            _basicData = _integratedClass.BasicData;
            _systemData = _integratedClass.SystemData;

            SaveParameterCommand = new RelayCommand(_basicData.SaveBasicData);
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

        public double Reference
        {
            get { return _basicData.Reference; }
            set
            {
                if (_basicData.Reference != value)
                {
                    _basicData.Reference = value;
                    OnPropertyChanged(nameof(Reference));
                }
            }
        }

        public double ProcessTime
        {
            get { return _systemData.ProcessTime; }
            set
            {
                if (_systemData.ProcessTime != value)
                {
                    _systemData.ProcessTime = value;
                    OnPropertyChanged(nameof(ProcessTime));
                }
            }
        }

        public RelayCommand SaveParameterCommand { get; set; }
    }
}
