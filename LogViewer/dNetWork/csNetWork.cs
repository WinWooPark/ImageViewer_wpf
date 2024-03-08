using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using System.Timers;
using System.Text;
using System.IO;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using dNetwork;

namespace dNetWork
{
    public static class Constant 
    {
        public const int ServerMode = 0;
        public const int ClientMode = 1;
    }

    public class ClientInfo //서버에서 가지고 있는 Client 정보
    {
        System.Timers.Timer                  _Timer;
        TcpClient?                           _Client;
        NetworkStream?                       _ClientStrem;
        csNetWork?                           _Server;
        string?                              _ClientIP;
        Thread?                              _RecvThread;
        bool                                 _IsRecvThreadRun = false;
        int                                  _ClientPort;
        event EventHandler                   _ClientDisconnet;
        AutoResetEvent                     _ThreadExit;

        public int ClientPort { get { return _ClientPort; } }
        public string ClientIP { get { return _ClientIP; } }
        public TcpClient Client { get { return _Client; } }
        public NetworkStream ClientStrem { get { return _ClientStrem; } }

        public Thread RecvThread { get { return _RecvThread; } set{ _RecvThread = value; } }

        public void startHeartbeatTimer(bool flag) 
        {
            if (flag)
            {
                _Timer = new System.Timers.Timer();
                _Timer.Interval = 5000; // 초
                _Timer.Elapsed += new ElapsedEventHandler(SendHeartbeat);
                _Timer.Start();
            }
            else 
            {
                
                _Timer.Close();
            }
            
        }

        void SendHeartbeat(object sender, ElapsedEventArgs e) 
        {
            _Server.ServerToClientSendData(_ClientPort, "", Constante._Heartbeat);
        }

        public void CreateClientInfo(TcpClient Client, csNetWork Server, EventHandler EventClient)
        {
            this._Client = Client;
            this._Server = Server;
            this._ClientIP = ((IPEndPoint)_Client.Client.RemoteEndPoint).Address.ToString(); //연결된 Client IP
            this._ClientPort = ((IPEndPoint)_Client.Client.RemoteEndPoint).Port; //포트 번호로 식별
            this._ClientStrem = Client.GetStream();

            this._RecvThread = new Thread(Thread_Recv);
            this._IsRecvThreadRun = true;
            this._RecvThread.Start();
            
            this._ClientDisconnet = EventClient;

            _ThreadExit = new AutoResetEvent(false);
         
            // startHeartbeatTimer(true);
        }

        public void CloseClient() 
        {
            //startHeartbeatTimer(false);

            _Client.Close();
            _Client.Dispose();

            _ClientStrem.Close();
            _ClientStrem.Dispose();

            _IsRecvThreadRun = false;
            _ThreadExit.WaitOne(1000);
        }
        private void Thread_Recv() 
        {
            bool Exit = false;

            while (_IsRecvThreadRun == true) 
            {
                //여기서 Client 에서 날아온 데이터를 읽는다.
                //날아온 메시지 헤더에 IP주소를 입력해서 메시지 루프에 추가한다.

                int bytesRead = 0;
                string message = null;

                //Client 마다 Thread를 하나씩 만들었기 때문에 Async로 안해도 된다.
                //Read 함수에서 계속 대기 하다가 데이터가 들어왔을때 메시지 루프에 추가 하도록 한다..
                try
                {
                    byte[] sizeBuffer = new byte[5];

                    bytesRead = _ClientStrem.Read(sizeBuffer, 0, sizeBuffer.Length);

                    int length = BitConverter.ToInt32(sizeBuffer, 0);

                    byte[] buffer = new byte[length];
                    bytesRead = _ClientStrem.Read(buffer, 0, buffer.Length);

                    if (bytesRead <= 0) continue;

                    message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                }
                catch (IOException e)
                {
                    _Server._CallbackErrorFunc("Server Read Fail Please Ckeck Server Socket.");
                    _ClientDisconnet.Invoke(this, EventArgs.Empty);
                    continue;
                }

                csNetWorkPacket obj = csNetWorkPacket.DeserializePacket(message);

                if(obj == null) continue;

                //Client한테 받은 메시지 server Message Loop로 전달
                _Server.PushMessage(obj);

                Thread.Sleep(1);
            }
            _ThreadExit.Set();  
            return;
        }
    }

    // 로그 뷰어 -> 서버

    // Image 뷰어 -> 클라이언트
    public class csNetWork
    {
        object                                  _ClientSendlock;
        object                                  _ServerSendlock;

        int                                     _NetMode;
        string                                  _IP;
        public string IP { get { return _IP; } }
        int                                     _PortMun;
        int                                     _BufferSize;
        public int BufferSize { get { return _BufferSize; } }

        IPAddress                               _IpAddress;

        //server var
        TcpListener                             _Server;
        public List<ClientInfo>                 _ClientList;
        NetworkStream                           _ServerStrem;
        
        Thread                                  _AcceptClientThread;
        bool                                    _AcceptClientThreadRun = false;
        
        Thread                                  _MessageLoopThread;
        bool                                    _IsMeddageLoopRun;
        private object                          _MessageLooplock;
        ConcurrentQueue<csNetWorkPacket>        _MessageQueue;
        

        //Client var
        TcpClient                               _Client;
        NetworkStream                           _ClientStrem;
        Thread                                  _ClientRecvThread;
        protected bool                          _IsClientRecvRun;
        int                                     _clientMyPort;
        public int ClientMyPort { get { return _clientMyPort; } }


        public Action<string>                          _CallbackFunc;
        public Action<string>                          _CallbackErrorFunc;

        public event EventHandler                      _ClientDisconnet;
        public event EventHandler                      _ServerDisconnet;


        public csNetWork() { }

        public void initNetWork(int NetMode, string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 2048, bool MultiClient = false) 
        {
            //파라미터 내부로 복사
            _NetMode = NetMode;
            _IP = IP; 
            _PortMun = PortNum;
            _BufferSize = BufferSize;

            switch (NetMode) 
            {
                case Constant.ServerMode:
                    initServerNetWork(IP, PortNum, BufferSize, MultiClient);
                    break;
                case Constant.ClientMode:
                    initClientNetWork(IP, PortNum, BufferSize, MultiClient);
                    break;
            }
        }

        private void initServerNetWork(string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 2048, bool MultiClient = false) 
        {
            //string 으로 받은 IP 변환
            _IpAddress = IPAddress.Parse(IP);

            _Server = new TcpListener(_IpAddress, PortNum);
            if (_Server == null) return;

            //클라이언트의 연결 대기
            _Server.Start();

            _ClientList = new List<ClientInfo>();

            _ServerSendlock = new object();

            //다중 클라이언트 연결을 위해 Accept 내용 Thread로 루틴 분리.
            _AcceptClientThread = new Thread(Thread_MultiClientAccept);
            _AcceptClientThreadRun = true;
            _AcceptClientThread.Start();

            //Client에서 날아온 메시지 처리 Thread 생성
            _MessageLooplock = new object();
            _MessageQueue = new ConcurrentQueue<csNetWorkPacket>();
            _MessageLoopThread = new Thread(Thread_MessageLoop);
            _IsMeddageLoopRun = true;
            _MessageLoopThread.Start();
        }
        private void initClientNetWork(string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 2048, bool MultiClient = false)
        {
            if(_Client == null)
                _Client = new TcpClient();

            if (_Client == null) return;

            int Count = 0;
            do
            {
                if (Count == 1000) return;

                try
                {
                    _Client.Connect(IP, PortNum);
                }
                catch 
                {
                    Thread.Sleep(100);
                    Count++;
                }
            }
            while (_Client.Connected == false); //클라이언트가 먼저 켜져 있으면 서버가 켜질때 까지 무한 루틴 대기.

            _clientMyPort = ((IPEndPoint)_Client.Client.LocalEndPoint).Port;
            _ClientStrem = _Client.GetStream();

            if(_ClientSendlock == null)
                _ClientSendlock = new object();

            if(_ClientRecvThread == null)
                _ClientRecvThread = new Thread(Thread_CilentRecv);
            _IsClientRecvRun = true;
            _ClientRecvThread.Start();

        }

        void Thread_CilentRecv() 
        {
            while (_IsClientRecvRun == true) 
            {
                string message = null;
                int bytesRead = 0;

                //Client 마다 Thread를 하나씩 만들었기 때문에 Async로 안해도 된다.
                //Read 함수에서 계속 대기 하다가 데이터가 들어왔을때 메시지 루프에 추가 하도록 한다..
                try
                {
                    byte[] sizeBuffer = new byte[4];

                    bytesRead = _ClientStrem.Read(sizeBuffer, 0, sizeBuffer.Length);

                    int length = BitConverter.ToInt32(sizeBuffer, 0);

                    byte[] buffer = new byte[length];
                    bytesRead = _ClientStrem.Read(buffer, 0, buffer.Length);

                    if (bytesRead <= 0) continue;

                    message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                }
                catch (IOException e)
                {
                    _CallbackErrorFunc("Server Read Fail Please Ckeck Server Socket.");
                };

                csNetWorkPacket packet = csNetWorkPacket.DeserializePacket(message);
                if (packet == null) continue;

                switch (packet.Type)
                {
                    case Constante._sHeartbeat:
                        ClientToServerSendData("", Constante._Heartbeat);
                        break;

                    case Constante._sData:
                        
                        break;

                    case Constante._sACK:
                        break;

                    case Constante._sDataLength:
                        break;

                    case Constante._sExit:
                        ExitClientNetWork();
                        break;
                }
            }
        }

        public void ExitNetWork() 
        {
            switch (_NetMode)
            {
                case Constant.ServerMode:
                    ExitServerNetWork();
                    break;
                case Constant.ClientMode:
                    ExitClientNetWork();
                    break;
            }
        }

        private void ExitServerNetWork() 
        {
            BroadCasting("", Constante._Exit);
            //1. 연결된 Client 모두 종료
            foreach (ClientInfo client in _ClientList) client.CloseClient();

            //2. 서버의 Listen 상태 및 Strem 종료
            if (_Server != null) 
            {
                _Server.Stop();
                _Server.Dispose();
            }
            
            if(_ServerStrem != null) _ServerStrem.Close();

            //3. Accept Thread 종료
            _AcceptClientThreadRun = false;
            _AcceptClientThread.Join();

            //4. Clinet List 클리어
            _ClientList.Clear();

            //5. MessageQueue Clear
            _MessageQueue.Clear();

            //5. MessageQueue thread 종료
            _IsMeddageLoopRun = false;
            _MessageLoopThread.Join();
        }

        private void ExitClientNetWork()
        {
            //1. Client 소켓 종료
            _Client.Close();
            _Client.Dispose();

            //3. Strem 종료
            _ClientStrem.Close();
            _ClientStrem.Dispose();

            //2. Recv Thread 종료
            _IsClientRecvRun = false;
            _ClientRecvThread.Join();

        }

        public void ClientToServerSendData(string messageToSend ,int type = Constante._Data) 
        {
            lock (_ClientSendlock) 
            {
                //NetWork 단에서 한번 더 감싸서 보낸다.
                string packet = csNetWorkPacket.CreateNetWorkPacket(_NetMode, _IP, _clientMyPort, type, messageToSend);

                byte[] data = null;

                try 
                {
                    //가변 데이터를 보내기 위해 데이터 길이 부터 보낸뒤
                    string DataLength = packet.Length.ToString();
                    data = Encoding.ASCII.GetBytes(DataLength);
                    _ClientStrem.Write(data, 0, data.Length);
                    _ClientStrem.Flush();

                    Thread.Sleep(10);

                    // Main Data을 읽는다.
                    data = Encoding.ASCII.GetBytes(packet);
                    _ClientStrem.Write(data, 0, data.Length); // 데이터 전송
                    _ClientStrem.Flush();
                }
                catch (IOException e)
                {
                    _CallbackErrorFunc("Server Wirte Fail Please Check Client Socket");
                    
                    AbnormalServerTermination();

                    return;
                }

            }
        }

        public void ServerToClientSendData(int ProtNum , string messageToSend, int type = Constante._Data)
        {
            lock (_ServerSendlock)
            {
                //받은 메시지를 Json으로 패킷 형태로 보낸다.
                string packet = csNetWorkPacket.CreateNetWorkPacket(_NetMode, _IP, _PortMun, type, messageToSend);

                //Port 번호로 식별하여 보낼 Client를 찾는다.
                ClientInfo clientinfo = _ClientList.Find(client => client.ClientPort == ProtNum);

                if (clientinfo == null) return;

                byte[] data = null;
                try 
                {
                    //가변 데이터를 보내기 위해 데이터 길이 부터 보낸뒤
                    string DataLength = packet.Length.ToString();
                    data = Encoding.ASCII.GetBytes(DataLength);
                    clientinfo.ClientStrem.Write(data, 0, data.Length);
                    clientinfo.ClientStrem.Flush();

                    // Main Data을 읽는다.
                    data = Encoding.ASCII.GetBytes(packet);
                    clientinfo.ClientStrem.Write(data, 0, data.Length); // 데이터 전송
                    clientinfo.ClientStrem.Flush();
                }
                catch (IOException e) 
                {
                    _CallbackErrorFunc("Client Wirte Fail Please Check Client Socket");
                    AbnormalClientTermination(clientinfo);
                    return;
                }
                
            }
        }

        void BroadCasting(string messageToSend, int type = Constante._Data) 
        {
            lock (_ServerSendlock)
            {
                //받은 메시지를 Json으로 패킷 형태로 보낸다.
                string packet = csNetWorkPacket.CreateNetWorkPacket(_NetMode, _IP, _PortMun, type, messageToSend);

                //Port 번호로 식별하여 보낼 Client를 찾는다.
                foreach (ClientInfo client in _ClientList)
                {
                    if (client == null) return;

                    byte[] data = null;

                    //가변 데이터를 보내기 위해 데이터 길이 부터 보낸뒤
                    string DataLength = packet.Length.ToString();
                    data = Encoding.ASCII.GetBytes(DataLength);
                    client.ClientStrem.Write(data, 0, data.Length);
                    client.ClientStrem.Flush();

                    // Main Data을 읽는다.
                    data = Encoding.ASCII.GetBytes(packet);
                    client.ClientStrem.Write(data, 0, data.Length); // 데이터 전송
                    client.ClientStrem.Flush();
                }
            }
        }

        public  void AbnormalClientTermination(ClientInfo Error) 
        {
            Error.CloseClient();

            _ClientList.Remove(Error);
        }

        public void AbnormalServerTermination()
        {
            //서버가 닫치는 에러가 나면 소켓을 모두 닫고, 다시 서버와 연결을 시도한다. 이때 1000번 연결 시도하고 나서 안되면 return 해버린다.
            ExitClientNetWork();
            initClientNetWork(_IP, _PortMun, _BufferSize);
        }

        private void RemoveAndCloseCilent(csNetWorkPacket packet)
        {
            ServerToClientSendData(packet.Prot, "", Constante._Exit);

            ClientInfo Info = _ClientList.Find(client => client.ClientPort == packet.Prot);

            Info.CloseClient();

            _ClientList.Remove(Info);
        }

        private void Thread_MultiClientAccept() 
        {
            while (_AcceptClientThreadRun == true) 
            {
                try
                {
                    //서버에 연결 요청된 Client 반환
                    TcpClient client = _Server.AcceptTcpClient();

                    //다중 클라이언트 관리를 하기 위해 Client info class 생성
                    ClientInfo info = new ClientInfo();
                    _ClientDisconnet = new EventHandler(EventAbnormalClientTermination);

                    info.CreateClientInfo(client, this, _ClientDisconnet);

                    //생성한 각각 Client 마다 각자 Recv thread 생성
                    _ClientList.Add(info);
                }
                catch (Exception ex)
                {
                    // 연결이 안될경우 모든 Client를 닫아 버린다.
                    foreach (ClientInfo client in _ClientList) client.CloseClient();
                        _ClientList.Clear();
                    
                    return;
                }
            }
        }

        public void ErrorCallBack(Action<string> ErrorMsg) { _CallbackErrorFunc = ErrorMsg; }

        public void MessageLoopCallBack(Action<string> callback) { _CallbackFunc = callback; }

        public void PushMessage(csNetWorkPacket Packet) 
        {
            //ConcurrentQueue<string> push / pop 은 모두 thread free 하다.
            _MessageQueue.Enqueue(Packet);
        }

        private void Thread_MessageLoop() 
        {
            csNetWorkPacket packet;
            while (_IsMeddageLoopRun == true) 
            {
                lock (_MessageLooplock) 
                {
                    if (_MessageQueue.IsEmpty == true) continue; //Queue가 비어있으면 아래 동작 아무것도 안함 ㅋ.

                    if (_MessageQueue.TryDequeue(out packet) == false) continue;

                    switch (packet.Type) 
                    {
                        case Constante._sHeartbeat:
                            //ServerToClientSendData(packet.Prot, "", Constante._Heartbeat);
                           
                            break;

                        case Constante._sData:
                            _CallbackFunc(packet.Data);
                            break;

                        case Constante._sACK:
                            break;

                        case Constante._sDataLength:
                            break;

                        case Constante._sExit:
                            break;
                    }

                }
            }

            return;
        }

        void EventAbnormalClientTermination(object sender, EventArgs e)
        {
            AbnormalClientTermination((ClientInfo)sender);
        }

        void EventAbnormalServerTermination(object sender, EventArgs e)
        {
            Console.WriteLine("The threshold was reached.");
            Environment.Exit(0);
        }

    } // NetWork Class End
} //namespace end
