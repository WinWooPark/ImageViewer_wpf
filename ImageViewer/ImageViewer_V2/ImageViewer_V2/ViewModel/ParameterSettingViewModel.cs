using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageViewer_V2.Model.Data;
using ImageViewer_V2.Model.MainSystem;

namespace ImageViewer_V2.ViewModel
{
    public class ParameterSettingViewModel : ObservableObject
    {
        IntegratedClass _integratedClass;
        BasicData _basicData;
        MainSystem _mainSystem;

        public ParameterSettingViewModel()
        {
            _mainSystem = MainSystem.Instance;
            _mainSystem.ViewModels.Add(this.GetType().Name, this);

            _integratedClass = IntegratedClass.Instance;
            _basicData = _integratedClass.BasicData;

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
            get { return _basicData.ProcessTime; }
            set
            {
                if (_basicData.ProcessTime != value)
                {
                    _basicData.ProcessTime = value;
                    OnPropertyChanged(nameof(ProcessTime));
                }
            }
        }

        public RelayCommand SaveParameterCommand { get; set; }
    }
}
