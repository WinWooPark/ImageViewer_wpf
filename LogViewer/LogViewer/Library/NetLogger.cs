using dNetWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dNetwork;
using LogViewer.Library;
using System.Xml;

namespace dNetwork
{
    public class csNetLogger : csNetWork
    {
        /// <Singleton>
        private static csNetLogger? Instance = null;

        // 생성자를 private로 선언하여 외부에서 직접 생성 막음
        public csNetLogger(int NetMode, string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 1024, bool MultiClient = false)
        {
            initNetWork(NetMode, IP, PortNum, BufferSize, MultiClient);
            lockObject = new object();
            //MessageLoopCallBack(LogMassageCallBack);
        }

        ~csNetLogger()
        {
            ExitNetWork();
        }

        public static csNetLogger CreateInstance(int NetMode, string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 1024, bool MultiClient = false)
        {
            if (Instance == null) Instance = new csNetLogger(NetMode, IP, PortNum, BufferSize, MultiClient);

            return Instance;
        }

        public static csNetLogger Logger { get { return Instance; } }
        /// </Singleton>

        private object lockObject;


        public void Log(int Priority,string Msg) 
        {
            lock (lockObject) 
            {
                string SendMag = LogData.CreateLogData(IP, Priority, Msg);
                ClientToServerSendData(SendMag);
            }
        }

        public void LogMassageCallBack(string str) 
        {
            string Data = str;

            XmlDocument XML = new XmlDocument();
            XML.LoadXml(Data);

            XmlElement root = XML.DocumentElement;

            foreach (XmlNode childNode in root.ChildNodes)
            {
                if (childNode is XmlElement)
                {
                    XmlElement childElement = (XmlElement)childNode;
                    string TESt = childElement.InnerText;
                }
            }

        }

    }
}
