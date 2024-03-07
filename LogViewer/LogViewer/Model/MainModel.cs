using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using LogViewer.SystemLib;
using System.Collections.Concurrent;
using System.Web;
using System.Net.Sockets;
using dNetwork;

namespace LogViewer.Model
{
    class MainModel
    {
        ConcurrentQueue<LogData> _logData;
        csIntegrated _integrated = null;
        public csIntegrated Integrated { get { return _integrated; } }

        public MainModel() 
        {
            _integrated = csIntegrated.Instance; //dll 내부에 있는 내용들을 딱 한번만 생성해서 여기다 묵어 둔다.
            _integrated.initIntegratedclass();
            _logData = new ConcurrentQueue<LogData>(); //Client에서 받은 로그들은 여기에다가 쌓아둔다.
            _logData.Clear();
        }

        public LogData DeserializePacketAndPushQueue(string Packet) 
        {
            //json string으로 날라온 데이터를 LogData로 변환;
            LogData Data = LogData.DeserializePacket(Packet);
            _logData.Enqueue(Data); // 파일 저장에 사용하기 위해 넣어둔다.

            return Data;
        }

        public void CloseProgram() 
        {
            _integrated.DeleteIntegratedClass();
        }
    }

   
}
