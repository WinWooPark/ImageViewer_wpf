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
using System.Xml;
using System.Data.SqlTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Web;
using Microsoft.VisualBasic;
using System.Windows.Shapes;
using dNetwork;
using System.Windows;

namespace LogViewer.ViewModel
{
    class MainViewerModel : INotifyPropertyChanged
    {
        private ObservableCollection<LogData> _LogData;

        public ObservableCollection<LogData> LogData
        {
            get { return _LogData; }
            set
            {
                if(LogData != value) { _LogData = value; }

                OnPropertyChanged(nameof(LogData));
            }
        }

        MainModel _mainModel = null;
        public MainViewerModel()
        {
            LogData = new ObservableCollection<LogData> ();

            _mainModel = new MainModel ();
            _mainModel.Integrated.Server.MessageLoopCallBack(CallbackLogMessage);
            _mainModel.Integrated.Server.ErrorCallBack(CallbackLogMessage);
        }

        ~MainViewerModel()
        {
        
        }

        void CallbackErrorMessage(string Msg) 
        {
            MessageBox.Show(Msg);
        }

        public void CallbackLogMessage(string Packet)
        {
            LogData Data = _mainModel.DeserializePacketAndPushQueue(Packet);

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                // UI 업데이트 코드
                UpdataUI(Data);
            }
            );
        }

        public void CloseViewModel() 
        {
            _mainModel.CloseProgram();
        }


        void UpdataUI(LogData Data) 
        {
            _LogData.Add(Data);

            if (_LogData.Count == 100) _LogData.RemoveAt(0);
            
        }

        /// <Event Handler>
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// </Event Handler>
    }
}
