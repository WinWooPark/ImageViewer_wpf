using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Reflection.PortableExecutable;
using System.Web;
using System.Text;


// ����� ���� �޸𸮸� �Ҵ��ϰ� , ����� �а� ���� �ϴ� ������ ���´�.
// string ���� �����͸� �ѱ���̱� ������ ��� ���� �ڽ� class���� ������ �����͸� �Ľ��ؼ� �޸𸮿� ����
// ������ �Ľ̿��� �о���°����� ����.. 

namespace dSharedMemory
{
    static public class Constants
    {
        public const int ReadMode = 0;
        public const int WriteMode = 1;
    }

    public class csSharedMemory
    {
       protected MemoryMappedFile                            _Mmf;
       protected MemoryMappedViewStream                      _Stream;

       protected Mutex                                       _Mutex;

       protected BinaryReader                                _Reader;
       protected BinaryWriter                                _Writer;

        int                                                  _Mode;
        int                                                  _BufferSize;
        byte[]                                               _Buffer;
        int                                                  _Memsize;

        public void AllocSharedMemory(int MemMode, string MemName = "MemShared",string MutName = "MutName", int MemSize = 4096, int BufferSize = 4096) 
        {
            //�ܺ� �Ķ���� ���� ������ ����
            _Mode = MemMode;
            _Memsize = MemSize;
            _BufferSize = BufferSize;

            //mutex ����
            bool flag = false;
            _Mutex = new Mutex(true, MutName, out flag);
            if (flag == false) return;
            _Mutex.ReleaseMutex(); //�ʱ⿡ ��� �ִ� Mutex Release

            //mmf ����
           _Mmf = MemoryMappedFile.CreateNew(MemName, MemSize);

            _Stream = _Mmf.CreateViewStream();

            switch (MemMode) 
            {
                case Constants.ReadMode:
                    _Reader = new BinaryReader(_Stream);
                    break;
                case Constants.WriteMode:
                    _Writer = new BinaryWriter(_Stream);
                    break;
            }
        }

        public void deleteSharedMemory() 
        {
            _Mutex.ReleaseMutex();
            _Mutex.Dispose();

            switch (_Mode)
            {
                case Constants.ReadMode:
                    _Reader.Close();
                    _Reader.Dispose();
                    break;
                case Constants.WriteMode:
                    _Writer.Close();
                    _Writer.Dispose();
                    break;
            }

            _Stream.Close();
            _Stream.Dispose();

            _Mmf.Dispose();
        }

        void WriteSharedMemory(string Data) 
        {
            _Mutex.WaitOne(); //mutex Lock

            if(Data.Length < _Memsize)
                _Writer.Write(Data);

            _Mutex.ReleaseMutex(); //mutex UnLock 
        }

        void ReadSharedMemory(ref string Data) 
        {
            _Mutex.WaitOne(); //mutex Lock

            Data = _Reader.ReadString();

            _Mutex.ReleaseMutex(); //mutex UnLock 

        }

        //byte[] StringToBytes(string input, Encoding encoding)
        //{
        //    return encoding.GetBytes(input);
        //}

        //string BytesToString(byte[] inputBytes, Encoding encoding)
        //{
        //    return encoding.GetString(inputBytes);
        //}

    }

}
