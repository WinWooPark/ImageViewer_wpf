using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using System.Text;

namespace dNetWork
{
    public static class Constant 
    {
        public const int ServerMode = 0;
        public const int ClientMode = 1;
    }

    public class NetWorkPacket
    {
        public NetWorkPacket(){}
        void GetBuffer(string Msg) 
        {
            int DataLength = Msg.Length;
            _Buffer = new byte[DataLength];
            _Buffer = Encoding.UTF8.GetBytes(Msg);
        }

        byte            _StratBit = 0x01;
        byte            _DataLength;
        byte[]          _Buffer;
        byte            _EndBit = 0x03;
    }

    public class csNetWork
    {
        object                                  _lockPacketObj;
        object                                  _lockServerObj;
        object                                  _lockClientObj;


        int                                     _NetMode;
        string                                  _IP;
        int                                     _PortMun;
        ConcurrentQueue<NetWorkPacket>          _QueuePacket;

        IPAddress                               _IpAddress;

        //server var
        TcpListener                             _Server;

        List<TcpClient>                         _ClientList;
        Thread                                  _ServerRecvThread;
        bool                                    _IsServerRecvRun;

        //Client var
        TcpClient                               _Client;
        Thread                                  _ClientRecvThread;
        bool                                    _IsClientRecvRun;
        public csNetWork() { }

        public void initNetWork(int NetMode, string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 1024, bool MultiClient = false) 
        {
            //파라미터 내부로 복사
            _NetMode = NetMode;
            _IP = IP; 
            _PortMun = PortNum;

            // 비동기로 패킷을 보내기 위해 , Queue 추가
            _QueuePacket = new ConcurrentQueue<NetWorkPacket>();
            _QueuePacket.Clear();

            _lockPacketObj = new object();
            _lockServerObj = new object();
            _lockClientObj = new object();

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

        public void AddSendPacket(NetWorkPacket Packet)
        {
            //ConcurrentQueue thread Free 하기 때문에 굳이 동기화 안한다.
            //큐에 데이터를 집어 넣는다.
            _QueuePacket.Enqueue(Packet);
        }

        private void initServerNetWork(string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 1024 , bool MultiClient = false) 
        {
            //string 으로 받은 IP 변환
            _IpAddress = IPAddress.Parse(IP);

            _Server = new TcpListener(_IpAddress, PortNum);
            if (_Server == null) return;

            //클라이언트의 연결 대기
            _Server.Start();

            if (MultiClient == true)
            {
                _Server.BeginAcceptTcpClient(MultiClientConnection, null);

            }
            else 
            {

            }
        }
        private void initClientNetWork(string IP = "127.0.0.1", int PortNum = 5000, int BufferSize = 1024, bool MultiClient = false)
        {
            _Client = new TcpClient(IP, PortNum);

            if (_Client == null) return;

            _Client.Connect(IP, PortNum);
        }

        //private void ServerSendThread();
        private void ServerRecvThread() 
        {
            while (_IsServerRecvRun == true)
            {

            }
            return;
        }

        private void ClientRecvThread() 
        {
            while (_IsClientRecvRun == true)
            {

            }

            return;
        }

        private void MultiClientConnection(IAsyncResult result) 
        {
            try
            {
                // Accept the new client connection
                TcpClient client = _Server.EndAcceptTcpClient(result);

                // Add the client to the list of connected clients
                _ClientList.Add(client);

                // Start handling communication with the client asynchronously
                Thread clientThread = new Thread(() => HandleClientCommunication(client));
                clientThread.Start();

                // Continue accepting more client connections
                _Server.BeginAcceptTcpClient(MultiClientConnection, null);
            }
            catch (Exception ex)
            {
                // 연결이 안될경우 모든 Client를 닫아 버린다.
                foreach (TcpClient client in _ClientList) 
                {
                    client.Close();
                    return;
                }
            }
        }

        private void ClientCommunication(TcpClient client) 
        {

        }

        private void SendMsgThreadForClient() 
        {
            
        }
    }
}
