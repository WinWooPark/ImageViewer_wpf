using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace LibNetWork.Server
{
    public class ClientHandler()
    {
        string _ip;
        int _port;
        TcpClient _client;
        NetworkStream _NetStream;
       
        NetworkServer _host;
        NetworkStream _hostStream;
        Stopwatch _stopwatch;

        //Recv
        Thread              _RecvThread;
        bool                _IsRecvRun;
        AutoResetEvent      _RecvThreadExit;

        //Send
        Thread              _SendThread;
        bool                _IsSendRun;
        AutoResetEvent      _SendThreadExit;
        AutoResetEvent      _AddSendData;
        ConcurrentQueue<byte[]> _SendQueue;

        public void StartStopwatch() 
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public double StopStopwatch()
        {
            double Interval = 0;
            _stopwatch.Stop();

            Interval = (double)_stopwatch.ElapsedMilliseconds;

            return Interval;
        }

        public void InitClientHandler(TcpClient client, NetworkServer Host)
        {
            _client = client;
            _ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            _port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
            _NetStream = client.GetStream();
            _host = Host;

            _stopwatch = new Stopwatch();

            //Thread 관련
            _RecvThreadExit = new AutoResetEvent(false);
            _RecvThread = new Thread(Thread_Recv);
            _IsRecvRun = true;
            _RecvThread.Start();

            _SendQueue = new ConcurrentQueue<byte[]>();
            _SendQueue.Clear();

            _SendThreadExit = new AutoResetEvent(false);
            _AddSendData = new AutoResetEvent(false); 

            _SendThread = new Thread(Thread_Send);
            _IsSendRun = true;
            _SendThread.Start();
        }

        public void CloseClientHandler()
        {
            if (_NetStream != null)
            {
                _NetStream.Close();
                _NetStream.Dispose();
            }

            if (_client != null)
            {
                _client.Close();
                _client.Dispose();
            }

            _IsRecvRun = false;
            _RecvThreadExit.WaitOne();

            _IsSendRun = false;
            _SendThreadExit.WaitOne();
        }

        void Thread_Recv()
        {
            while (_IsRecvRun)
            {
                

            }

            _RecvThreadExit.Set();

            return;
        }

        void Thread_Send() 
        {
            byte[] data = null;

            while (_IsRecvRun)
            {
                _AddSendData.WaitOne();

                if (_SendQueue.IsEmpty) continue;

                if(_SendQueue.TryDequeue(out) == false) continue;




            }

            _SendThreadExit.Set();

            return ;
        }

        public void AddSend(byte[] Send) 
        {
            _SendQueue.Enqueue(Send);
            _AddSendData.Set();
        }
    }
}
