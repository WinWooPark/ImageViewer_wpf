using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//여기에서 XML 데이터를 만들자.
namespace LogViewer.Library
{

    public class LogData
    {
        public LogData() { }

        public static string CreateLogData(int Priority, string Msg, int FuncStep = 2)
        {
            string strDate = GetCurrentTime24Hour();
            string strFunc = GetPreFuntionName(FuncStep);
            string strPriority = GetPriority(Priority);
            string strLog_Msg = Msg;

            string SendMsg = string.Join(" ", strDate, strFunc, strPriority, strLog_Msg);
            return SendMsg;
        }

        public static string GetCurrentTime24Hour()
        {
            DateTime currentTime = DateTime.Now;
            string currentTime24Hour = currentTime.ToString("HH:mm:ss");

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
                case csConstant.PriHig:
                    Pri = "HIGHT";
                    break;

                case csConstant.PriMid:
                    Pri = "MID";
                    break;

                case csConstant.PriLow:
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
