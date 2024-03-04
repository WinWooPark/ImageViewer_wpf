using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using LogViewer.System;

namespace LogViewer.ViewModel
{
    class MainViewerModel : INotifyPropertyChanged
    {
        public MainViewerModel() 
        {
            Log = new LogData();
            Log._Date = "21.02.32";
            Log._Priority = "Hight";
            Log._Function = "VewModel";
            Log._Log_Message = "TEST중입니다.";
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        LogData Log;


        public string Date 
        {
            get { return Log._Date; }
            set 
            {
                if (Log._Date != value) 
                {
                    Log._Date = value;
                    OnPropertyChanged(Log._Date);
                }
            }
        }
        public string Function
        {
            get { return Log._Function; }
            set
            {
                if (Log._Function != value)
                {
                    Log._Function = value;
                    OnPropertyChanged(Log._Function);
                }
            }
        }
        public string Priority
        {
            get { return Log._Priority; }
            set
            {
                if (Log._Priority != value)
                {
                    Log._Priority = value;
                    OnPropertyChanged(Log._Priority);
                }
            }
        }
        public string Log_Message
        {
            get { return Log._Log_Message; }
            set
            {
                if (Log._Log_Message != value)
                {
                    Log._Log_Message = value;
                    OnPropertyChanged(Log._Log_Message);
                }
            }
        }



    }
}
