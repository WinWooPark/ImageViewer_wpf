using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;


//여기에서 XML 데이터를 만들자.
namespace dNetwork
{
    public class LogData
    {
        int _Port;
        public int Port { get { return _Port; } set { if (_Port != value) _Port = value; } }

        string _Date;
        public string Date { get { return _Date; } set { if (_Date != value) _Date = value; } }

        string _Func;
        public string Func { get { return _Func; } set { if (_Func != value) _Func = value; } }

        string _Priority;
        public string Priority { get { return _Priority; } set { if (_Priority != value) _Priority = value; } }

        string _Log_Msg;
        public string Log_Msg { get { return _Log_Msg; } set { if (_Log_Msg != value) _Log_Msg = value; } }

        public LogData() { }

        public static string CreateLogData(int Port, int Priority, string Msg, int FuncStep = 2)
        {
            LogData logData = new LogData();

            logData.Port = Port;
            logData.Date = GetCurrentTime24Hour();
            logData.Func = GetPreFuntionName(FuncStep);
            logData.Priority = GetPriority(Priority);
            logData.Log_Msg = Msg;

            string Packet = JsonConvert.SerializeObject(logData);

            return Packet;
        }

        public static LogData DeserializePacket(string Packet) 
        {
            if (Packet == null) return null;

            LogData obj = JsonConvert.DeserializeObject<LogData>(Packet);

            return obj;
        }

        public static string GetCurrentTime24Hour()
        {
            DateTime currentTime = DateTime.Now;
            string currentTime24Hour = currentTime.ToString("HH:mm:ss.fff");

            return currentTime24Hour;
        }

        public static string GetPreFuntionName(int Step = 2)
        {
            string FuncName = new StackFrame(Step, true).GetMethod().Name;
            return FuncName;
        }

        public static string GetPriority(int Priority) 
        {
            string Pri = "MID";

            switch (Priority) 
            {
                case dNetWork.csConstant.PriHig:
                    Pri = "HIGHT";
                    break;

                case dNetWork.csConstant.PriMid:
                    Pri = "MID";
                    break;

                case dNetWork.csConstant.PriLow:
                    Pri = "LOW";
                    break;

                default:
                    Pri = "MID";
                    break;
            }
            return Pri;
        }
    }
}
