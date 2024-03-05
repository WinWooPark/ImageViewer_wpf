using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Collections.ObjectModel;
using LogViewer.Model;
using dSharedMemory;
using System.Diagnostics.Eventing.Reader;

namespace LogViewer.ViewModel
{
    class MainViewerModel : INotifyPropertyChanged
    {
        csLogger LoggerRead;
        csLogger LoggerWirte;

        public MainViewerModel()
        {
            LoggerRead = new csLogger(0);
            LoggerWirte = new csLogger(1);
            
            LoggerWirte.Log(csLogger.GetCurrentMethodName(), 0, "TEST입니다.");

            string temp;
            LoggerRead.ReadSharedMemory(out temp);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
