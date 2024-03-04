using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Reflection.PortableExecutable;
using System.Web;
using System.Text;


// 여기는 공유 메모리를 할당하고 , 지우고 읽고 쓰고 하는 내용을 적는다.
// string 으로 데이터를 넘길것이기 때문에 상속 받은 자식 class에서 적절히 데이터를 파싱해서 메모리에 쓰고
// 적절히 파싱에서 읽어오는것으로 하자.. 

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
            //외부 파라미터 내부 변수로 복사
            _Mode = MemMode;
            _Memsize = MemSize;
            _BufferSize = BufferSize;

            //mutex 생성
            bool flag = false;
            _Mutex = new Mutex(true, MutName, out flag);
            if (flag == false) return;
            _Mutex.ReleaseMutex(); //초기에 잡고 있는 Mutex Release

            //mmf 생성
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
