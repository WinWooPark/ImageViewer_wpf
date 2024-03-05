using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Collections.ObjectModel;
using LogViewer.Model;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Interop;
using LogViewer.Library;
using dNetwork;

namespace LogViewer.ViewModel
{
    class MainViewerModel : INotifyPropertyChanged
    {
        public MainViewerModel()
        {
            Test();
        }

        void Test() 
        {
            //csNetLogger.CreateInstance(dNetWork.Constant.ServerMode);


            //csNetLogger.CreateInstance(dNetWork.Constant.ClientMode);

            csNetLogger Server = new csNetLogger(dNetWork.Constant.ServerMode);
            csNetLogger Client1 = new csNetLogger(dNetWork.Constant.ClientMode);
            csNetLogger Client2 = new csNetLogger(dNetWork.Constant.ClientMode);
            csNetLogger Client3 = new csNetLogger(dNetWork.Constant.ClientMode);
            csNetLogger Client4 = new csNetLogger(dNetWork.Constant.ClientMode);


            Client1.Log(csConstant.PriHig, "TEST Log Client1");
            Client2.Log(csConstant.PriHig, "TEST Log Client2");
            Client3.Log(csConstant.PriHig, "TEST Log Client3");
            Client4.Log(csConstant.PriHig, "TEST Log Client4");


            //csNetLogger.Logger.Log(csConstant.PriHig, "TEST Log");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
