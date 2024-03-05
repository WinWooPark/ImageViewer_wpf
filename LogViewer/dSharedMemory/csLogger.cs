using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dSharedMemory
{
    static class Constants
    {
        public const int Priority_Hight = 0;
        public const int Priority_Mid = 1;
        public const int Priority_Low = 2;
    }

    public class LogData
    {
        public LogData()
        {
            _Date = null;
            _Function = null;
            _Priority = null;
            _Log_Message = null;
        }
        public string? _Date;
        public string? _Function;
        public string? _Priority;
        public string? _Log_Message;
    }
    public class csLogger : csSharedMemory
    {
        static object lockObj = new object();

        const string _LogFilePath = "C:\\LogFile\\";

        Thread _WirteLogFileThread;
        bool _WirteLogFileThreadRun = false;
        ConcurrentQueue<LogData> _QueueLogData;

        public csLogger(int MemMode, string MemName = "MemShared", string MutName = "MutName", int MemSize = 4096, int BufferSize = 4096)
        {
            AllocSharedMemory(MemMode, MemName, MutName, MemSize, BufferSize);

            switch (MemMode)
            {
                case _ReadMode:
                    _QueueLogData = new ConcurrentQueue<LogData>();
                    _QueueLogData.Clear();

                    //메모리에서 읽은 데이터를 
                    _WirteLogFileThread = new Thread(WriteLogFile);
                    _WirteLogFileThreadRun = true;
                    _WirteLogFileThread.Start();

                    break;
                case _WriteMode:
                    break;
            }
        }

        public void Log(string FuncName, int Priority, string msg)
        {
            //외부에서는 Log 함수만 호출해서 해당 파라미터를 입력해주면 된다.
            //그럼 큐에 데이터를 할당하고 할당된 데이터를 파일에 하나씩 쓴다.
            Monitor.Enter(lockObj);

            LogData logdata = new LogData();
            string LogName = "default";

            logdata._Date = GetCurrentTime24Hour();
            logdata._Function = FuncName;
            logdata._Priority = GetPriority(Priority);
            logdata._Log_Message = msg;

            LogName = string.Join(" ", logdata._Date, logdata._Function, logdata._Priority, logdata._Log_Message);

            //_QueueLogData.Enqueue(LogName);
            WriteSharedMemory(LogName);

            Monitor.Exit(lockObj);
        }

        static public string GetCurrentMethodName()
        {
            // 현재 메서드의 이름 가져오기
            string methodName = "default";
            methodName = MethodBase.GetCurrentMethod().Name;

            return methodName;
        }

        private string GetCurrentTime24Hour() //내부에서 사용하는 메소드는 Private
        {
            DateTime currentTime = DateTime.Now;
            string currentTime24Hour = currentTime.ToString("HH:mm:ss");

            return currentTime24Hour;
        }

        private string GetPriority(int Priority) //내부에서 사용하는 메소드는 Private
        {
            string strpriority;
            switch (Priority)
            {
                case Constants.Priority_Hight:
                    strpriority = "HIGHT";
                    break;

                case Constants.Priority_Mid:
                    strpriority = "MID";
                    break;

                case Constants.Priority_Low:
                    strpriority = "LOW";
                    break;
                default:
                    strpriority = "MID";
                    break;
            }
            return strpriority;
        }
        private void WriteLogFile()
        {
            while (_WirteLogFileThreadRun)
            {
                if(_QueueLogData.IsEmpty == true) continue;

                

                Thread.Sleep(10);
            }
        }

        private string FileNameMaker(LogData logdata)
        {
            string FileName = "default";






            return FileName;
        }

      
    }
}
