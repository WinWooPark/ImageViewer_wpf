using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using System.Text;
using System.IO;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;

namespace dNetWork
{
    public static class Constant 
    {
        public const int ServerMode = 0;
        public const int ClientMode = 1;
    }

    public struct MessageData
    {
        public MessageData() { }

        public string IP = null;
        public string Msg = null;
        public TcpClient? _Client = null;
    }

    public class ClientInfo //서버에서 가지고 있는 Client 정보
    {
        TcpClient?                           _Client;
        NetworkStream?                       _ClientStrem;
        csNetWork?                           _Server;
        string?                              _ClientIP;
        Thread?                              _RecvThread;
        bool                                 _IsRecvThreadRun = false;
        int                                  _ClientPort;

        public int ClientPort { get { return _ClientPort; } }
        public string ClientIP { get { return _ClientIP; } }
        public TcpClient Client { get { return _Client; } }
        public NetworkStream ClientStrem { get { return _ClientStrem; } }

        public Thread RecvThread { get { return _RecvThread; } set{ _RecvThread = value; } }

        public void CreateClientInfo(TcpClient Client, csNetWork Server) 
        {
            this._Client = Client;
            this._Server = Server;
            this._ClientIP = ((IPEndPoint)_Client.Client.RemoteEndPoint).Address.ToString(); //연결된 Client IP
            this._ClientPort = ((IPEndPoint)_Client.Client.RemoteEndPoint).Port; //포트 번호로 식별
            this._ClientStrem = Client.GetStream();

            this._RecvThread = new Thread(Thread_Recv);
            this._IsRecvThreadRun = true;
            this._RecvThread.Start();
        }

        public void CloseClient() 
        {
            _IsRecvThreadRun = false;
            _RecvThread.Join();

            _ClientStrem.Close();
            _ClientStrem.Dispose();

            _Client.Close();
            _Client.Dispose();
        }
        private void Thread_Recv() 
        {
            byte[] buffer = new byte[_Server.BufferSize];

            while (_IsRecvThreadRun == true) 
            {
                //여기서 Client 에서 날아온 데이터를 읽는다.
                //날아온 메시지 헤더에 IP주소를 입력해서 메시지 루프에 추가한다.

                Array.Clear(buffer, 0x0, buffer.Length); //Thread 내부에서 동적으로 할당하지 않고 외부에서 한번 할당한다음에 메모리에 
                int bytesRead = 0;

                //Client 마다 Thread를 하나씩 만들었기 때문에 Async로 안해도 된다.
                //Read 함수에서 계속 대기 하다가 데이터가 들어왔을때 메시지 루프에 추가 하도록 한다..
                try 
                {
                    bytesRead = _ClientStrem.Read(buffer, 0, buffer.Length);
                    _ClientStrem.Flush();
                }
                catch { CloseClient(); };

                if (bytesRead <= 0) continue;

                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                //여기서 데이터 유효 검사를 해야한다. 데이터가 유효 하다면 리턴을 날리자.
                //데이터 유효성 검사 함수는 csNetWork 안에 있어야 하고, 데이터 유효 검사는 가상함수로 상속하자
                //무슨 데이터를 보낼지 모르니까 상속받은 곳에서 하는게 맞는거같다.

                MessageData msgData;
                msgData.IP = ClientIP;
                msgData.Msg = message;
                msgData._Client = _Client;

                if (msgData.IP == null || msgData.Msg == null) continue;
                //Client한테 받은 메시지 server Message Loop로 전달
                _Server.PushMessage(msgData);

                Thread.Sleep(100);
            }
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
        ConcurrentQueue<MessageData>             _MessageQueue;
        

        //Client var
        TcpClient                               _Client;
        NetworkStream                           _ClientStrem;
        Thread                                  _ClientRecvThread;
        protected bool                          _IsClientRecvRun;


        Action<string, string, string, string>  _Callback;
        public csNetWork() { }

        public void initNetWork(int NetMode, string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 1024, bool MultiClient = false) 
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

        private void initServerNetWork(string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 1024 , bool MultiClient = false) 
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
            _MessageQueue = new ConcurrentQueue<MessageData>();
            _MessageLoopThread = new Thread(Thread_MessageLoop);
            _IsMeddageLoopRun = true;
            _MessageLoopThread.Start();
        }
        private void initClientNetWork(string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 1024, bool MultiClient = false)
        {
            _Client = new TcpClient(IP, PortNum);            
            
            if (_Client == null) return;

            _ClientStrem = _Client.GetStream();

            _ClientSendlock = new object();

            _ClientRecvThread = new Thread(Thread_CilentRecv);
            _IsClientRecvRun = true;
            _ClientRecvThread.Start();

        }

        void Thread_CilentRecv() 
        {
            byte[] buffer = new byte[_BufferSize];

            while (_IsClientRecvRun == true) 
            {
                Array.Clear(buffer, 0x0, buffer.Length); //Thread 내부에서 동적으로 할당하지 않고 외부에서 한번 할당한다음에 메모리에 
                int bytesRead = 0;

                //Client 마다 Thread를 하나씩 만들었기 때문에 Async로 안해도 된다.
                //Read 함수에서 계속 대기 하다가 데이터가 들어왔을때 메시지 루프에 추가 하도록 한다..
                bytesRead = _ClientStrem.Read(buffer, 0, buffer.Length);
                if (bytesRead <= 0) continue;

                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                if (message == "ExitOK") 
                {
                    ExitClientNetWork();
                }
                if (message == "ACK") 
                {
                    int a = 0;
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
                    //ClientToServerSendData("Exit"); //Server로 종료 메시지 송신
                    ExitClientNetWork();
                    break;
            }
        }

        private void ExitServerNetWork() 
        {
            //1. 서버의 Listen 상태 및 Strem 종료
            _Server.Stop();
            _Server.Dispose();

            _ServerStrem.Close();

            //2. Accept Thread 종료
            _AcceptClientThreadRun = false;
            _AcceptClientThread.Join();

            //3. 연결된 Client 모두 종료
            foreach (ClientInfo client in _ClientList) client.CloseClient();

            //4. Clinet List 클리어
            _ClientList.Clear();

            //5. MessageQueue Clear
            _MessageQueue.Clear();

            //6. MessageQueue thread 종료
            _IsMeddageLoopRun = false;
            _MessageLoopThread.Join();
        }

        private void ExitClientNetWork()
        {
            //1. Recv Thread 종료
            _IsClientRecvRun = false;
            _ClientRecvThread.Join();

            ////2. Strem 종료
            _ClientStrem.Close();
            _ClientStrem.Dispose();

            //3. Client 소켓 종료
            _Client.Close();
            _Client.Dispose();
        }

        public void ClientToServerSendData(string messageToSend) 
        {
            lock (_ClientSendlock) 
            {
                byte[] data = Encoding.ASCII.GetBytes(messageToSend);

                _ClientStrem.Write(data, 0, data.Length); // 데이터 전송
            }
        }

        private void ServerToClientSendData(string IP , string messageToSend)
        {
            lock (_ServerSendlock)
            {
                byte[] data = Encoding.ASCII.GetBytes(messageToSend);

                ClientInfo Info = _ClientList.Find(client => client.ClientIP == IP);

                Info.ClientStrem.Write(data, 0, data.Length); // 데이터 전송
            }
        }

        private void RemoveAndCloseCilent(ref MessageData Msg) 
        {
            ServerToClientSendData(Msg.IP, "ExitOK");

            ClientInfo Info = _ClientList.Find(client => client.ClientIP == IP);

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
                    info.CreateClientInfo(client, this);

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

        public void MessageLoopCallBack(Action<string,string,string,string> callback) { _Callback = callback; }

        public void PushMessage(MessageData Msg) 
        {
            //ConcurrentQueue<string> push / pop 은 모두 thread free 하다.
            _MessageQueue.Enqueue(Msg);
        }

        private void Thread_MessageLoop() 
        {
            MessageData Msg;

            while (_IsMeddageLoopRun == true) 
            {
                lock (_MessageLooplock) 
                {
                    if (_MessageQueue.IsEmpty == true) continue; //Queue가 비어있으면 아래 동작 아무것도 안함 ㅋ.

                    if (_MessageQueue.TryDequeue(out Msg) == false) continue;

                    if (Msg.Msg == "Exit") RemoveAndCloseCilent(ref Msg);
                    else 
                    {
                        ServerToClientSendData(Msg.IP, "ACK");


                        string Data = Msg.Msg;

                        XmlDocument parsedXmlDoc = new XmlDocument();
                        parsedXmlDoc.LoadXml(Data);

                        XmlNode rootElement = parsedXmlDoc.SelectSingleNode("/Root");

                        // XPath를 사용하여 데이터 추출
                        XmlNode ipNode = parsedXmlDoc.SelectSingleNode("/Root/IP");
                        XmlNode dateNode = parsedXmlDoc.SelectSingleNode("/Root/Date");
                        XmlNode funcNode = parsedXmlDoc.SelectSingleNode("/Root/FuncName");
                        XmlNode priorityNode = parsedXmlDoc.SelectSingleNode("/Root/Priority");
                        XmlNode msgNode = parsedXmlDoc.SelectSingleNode("/Root/LogMsg");

                        // 데이터를 string 변수에 저장
                        string strip = ipNode.InnerText;
                        string strdate = dateNode.InnerText;
                        string strfunc = funcNode.InnerText;
                        string strpriority = priorityNode.InnerText;
                        string strmsg = msgNode.InnerText;


                        _Callback(strdate,strfunc,strpriority,strmsg);
                    }
                }
            }

        }
    }
}
