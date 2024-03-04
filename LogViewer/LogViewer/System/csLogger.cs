using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Reflection;
using dSharedMemory;


namespace LogViewer.System
{
    static class Constants
    {
        public const int Priority_Hight = 0;
        public const int Priority_Mid   = 1;
        public const int Priority_Low   = 2;
    }

    class LogData 
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
    class csLogger : csSharedMemory
    {
        static object                   lockObj = new object();

        const string                    _LogFilePath = "C:\\LogFile\\";

        Thread                          _WirteLogFileThread;
        bool                            _WirteLogFileThreadRun = false;
        ConcurrentQueue<LogData>        _QueueLogData;

        public csLogger(int MemMode, string MemName = "MemShared",string MutName = "MutName", int MemSize = 4096, int BufferSize = 4096) 
        {
            AllocSharedMemory(MemMode, MemName,MutName,MemSize,BufferSize);
            switch (MemMode) 
            {
                case dSharedMemory.Constants.ReadMode:
                    //컨커런트 큐 데이터 추가
                    _QueueLogData = new ConcurrentQueue<LogData>();
                    _QueueLogData.Clear();

                    _WirteLogFileThread = new Thread(WriteLogFile);
                    break;
                case dSharedMemory.Constants.WriteMode:
                    break;
            }
        }
        static string GetCurrentMethodName()
        {
            // 현재 메서드의 이름 가져오기
            string methodName = MethodBase.GetCurrentMethod().Name;

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
            switch(Priority) 
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
            while(_WirteLogFileThreadRun) 
            {
                
            }
        }
        public void Log(string FuncName, int Priority, string msg) 
        {
            //외부에서는 Log 함수만 호출해서 해당 파라미터를 입력해주면 된다.
            //그럼 큐에 데이터를 할당하고 할당된 데이터를 파일에 하나씩 쓴다.
            Monitor.Enter(lockObj);

            LogData logdata = new LogData();

            logdata._Date = GetCurrentTime24Hour();
            logdata._Function = FuncName;
            logdata._Priority = GetPriority(Priority);
            logdata._Log_Message = msg;

            _QueueLogData.Enqueue(logdata);

            Monitor.Exit(lockObj);
        }
    }
}
