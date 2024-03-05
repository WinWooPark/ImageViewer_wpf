﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Collections.ObjectModel;
using LogViewer.Model;
using System.Diagnostics.Eventing.Reader;

namespace LogViewer.ViewModel
{
    class MainViewerModel : INotifyPropertyChanged
    {
        public MainViewerModel()
        {
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
