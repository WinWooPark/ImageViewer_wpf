﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer_V2.Model.ManagementSystem;
using ImageViewer_V2.Model.Data;

namespace ImageViewer_V2.ViewModel
{
    public class MainFrameViewModel : ObservableObject
    {
        IntegratedClass _integratedClass;
        MainSystem _mainSystem;
        BasicData _basicData;

        public MainFrameViewModel()
        {
            _mainSystem = MainSystem.Instance;
            _integratedClass = _mainSystem.IntegratedClass;
            _integratedClass.MainFrameViewModel = this;
            _basicData = _integratedClass.BasicData;
            
        }

        public string Version 
        {
            get { return _basicData.Version; }
            set 
            {
                if (_basicData.Version != value) 
                {
                    _basicData.Version = value;
                    OnPropertyChanged(nameof(Version));
                }
            }
        }
    }
}
