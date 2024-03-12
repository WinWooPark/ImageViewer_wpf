using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using LibNetWork.CommonDefine;

namespace LibNetWork.Server
{
    public class NetworkServer
    {
        //서버 기본 정보
        string _hostIp;
        int _hostPort;
        int _bufferSize;
        IPAddress _address;
        TcpListener _tcpListener;

        List<ClientHandler> _clientHandlerList;


        Thread _AccpetThread;
        bool _IsAccpetRun = false;
        AutoResetEvent _AccpetThreadExit;

        Thread _messageLoopThread;
        bool _IsMessageLoopRun = false;

        //Property
        public string IP { get { return _hostIp; } }
        public int Port { get { return _hostPort; } }


        public NetworkServer()
        {
            //생성자에서 변수 초기화
            _hostIp = null;
            _hostPort = 0;
            _bufferSize = 0;
        }

        public void initNetWorkServer(string IP, int Port, EnumDefine.BufferSize Size)
        {
            //1. 변수 내부로 복사
            _hostIp = IP;
            _hostPort = Port;
            _bufferSize = (int)Size;
            _address = IPAddress.Parse(IP);

            //2. TCP Listener 생성
            _tcpListener = new TcpListener(_address, _hostPort);
            if (_tcpListener == null) return;

            //3. Listen 시작
            _tcpListener.Start();

            //4. Clinet List 생성
            _clientHandlerList = new List<ClientHandler>();
            _clientHandlerList.Clear();

            //5. 클라이언트 Accept Thread 시작
            _AccpetThreadExit = new AutoResetEvent(false);
            _AccpetThread = new Thread(Thread_ClientAccpet);
            _IsAccpetRun = true;
            _AccpetThread.Start();
        }

        public void closeNetWorkServer()
        {
            //1. 연결된 Client를 모두 종료한다.
            foreach (ClientHandler Handler in _clientHandlerList) Handler.CloseClientHandler();
            _clientHandlerList.Clear();

            //2. TcpListener 을 닫는다.
            _tcpListener.Stop();
            _tcpListener.Dispose();

            //3. Accpet Thread를 종료한다.
            _IsAccpetRun = false;
            _AccpetThreadExit.WaitOne();
        }

        void Thread_ClientAccpet()
        {
            while (_IsAccpetRun)
            {
                try
                {
                    TcpClient Client = _tcpListener.AcceptTcpClient();

                    ClientHandler Handler = new ClientHandler();
                    Handler.InitClientHandler(Client, this);

                    _clientHandlerList.Add(Handler);
                }
                catch (Exception ex)
                {
                    //Socket Error
                    foreach (ClientHandler Handler in _clientHandlerList) Handler.CloseClientHandler();

                    _clientHandlerList.Clear();
                }
            }
            _AccpetThreadExit.Set();
            return;
        }

        void checkHeartbeat(ClientHandler Handler) 
        {
          
        }

        















    }
}
