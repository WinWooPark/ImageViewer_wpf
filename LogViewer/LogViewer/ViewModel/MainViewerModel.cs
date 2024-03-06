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
using System.Xml;
using System.Data.SqlTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Web;
using Microsoft.VisualBasic;

namespace LogViewer.ViewModel
{
    public class csLogData
    {
        public string Date { get; set; }
        public string Func { get; set; }
        public string Priority { get; set; }
        public string Log_Msg { get; set; }
    }

    class MainViewerModel : INotifyPropertyChanged
    {
        private ObservableCollection<csLogData> _LogData;

        public ObservableCollection<csLogData> LogData
        {
            get { return _LogData; }
            set
            {
                if(LogData != value) { _LogData = value; }

                OnPropertyChanged(nameof(LogData));
            }
        }

        public MainViewerModel()
        {
            LogData = new ObservableCollection<csLogData> ();
            Test();

            //LogData = new ObservableCollection<csLogData>
            //{
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"},
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"},
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"},
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"},
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"},
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"},
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"},
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"},
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"},
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"},
            //    new csLogData{ Date ="12:55:12", Func = "FUNC" , Priority = "HIGHT" ,Log_Msg = "TEST"}
            //};
        }

        csNetLogger Server;
        

        void TestBinding(string str) 
        {

           


            //Dispatcher.Invoke(new Action<string,string, string,string>(UpdateUI),strdate, strfunc, strpriority, strmsg);

        }

        string strip;
        string strdate;
        string strfunc;
        string strpriority;
        string strmsg;


        private void UpdateUI(string strdate, string strfunc, string strpriority, string strmsg)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                // UI 업데이트 코드
               UItest(strdate, strfunc, strpriority, strmsg);
            });
        }

        void UItest(string strdate, string strfunc, string strpriority, string strmsg) 
        {
            LogData.Add(new csLogData { Date = strdate, Func = strfunc, Priority = strpriority, Log_Msg = strmsg });
        }

        public delegate void CallbackFunction(string s, string b, string c, string d);

        void Test() 
        {
           Server = new csNetLogger(dNetWork.Constant.ServerMode);

           // CallbackFunction CallBack = (string a, string b, string c, string d) => UpdateUI(a, b, c, d);
            Server.MessageLoopCallBack(UpdateUI);
            
        }

    

        public event PropertyChangedEventHandler? PropertyChanged;
        
        //private void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
