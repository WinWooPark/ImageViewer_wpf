using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer.CommonDefine;
using System.Windows.Input;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using ImageViewer.MainSystem;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.Xpo;

namespace ImageViewer.ViewModel
{
    class MainViewModel : INotifyPropertyChanged, ICommand
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CanExecuteChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool CanExecute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public MainViewModel() 
        {
            Version = CommonDefine.CommonDefine.Version;
            _systemInfo = new SystemInfo();
            _systemInfo.GetViewModel(this);
        }

        SystemInfo _systemInfo;

        string _version;
        public string Version 
        {
            get { return _version; }
            set 
            {
                if (_version != value) 
                {
                    _version = value;
                    OnPropertyChanged(nameof(Version));
                }
            }
        }

        int _threshold;
        public int Threshold 
        {
            get { return _threshold; }
            set 
            {
                if (_threshold != value)
                {
                    _threshold = value;
                    OnPropertyChanged(nameof(Threshold));
                }
            }
        }

        ICommand _ImageLoad;
        public ICommand ImageLoad
        {
            get
            {
                if (_ImageLoad == null)
                    _ImageLoad = new RelayCommand(TEST);
                return _ImageLoad;
            }
        }

        void TEST() 
        {
            int a = _threshold;
            int b = _threshold;
        }

        ICommand _ImageSave;
        public ICommand ImagSave
        {
            get
            {
                if (_ImageSave == null)
                    _ImageSave = new RelayCommand(_systemInfo.TESTViewmodel);
                return _ImageSave;
            }
        }
        //ICommand _ImageChange;
        //public ICommand ImageChange
        //{
        //    get
        //    {
        //        if (_ImageChange == null)
        //            _ImageChange = new RelayCommand();
        //        return _ImageChange;
        //    }
        //}



    }
}
