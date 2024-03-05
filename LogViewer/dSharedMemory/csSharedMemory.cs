using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Diagnostics.Metrics;

// 여기는 공유 메모리를 할당하고 , 지우고 읽고 쓰고 하는 내용을 적는다.
// string 으로 데이터를 넘길것이기 때문에 상속 받은 자식 class에서 적절히 데이터를 파싱해서 메모리에 쓰고
// 적절히 파싱에서 읽어오는것으로 하자.. 

namespace dSharedMemory
{
   
    public class csSharedMemory
    {
       MemoryMappedFile?                            _Mmf = null;
       MemoryMappedViewStream?                      _Stream = null;

       Mutex?                                       _Mutex = null;

       BinaryReader?                                _Reader = null;
       BinaryWriter?                                _Writer = null;

       int?                                         _Mode;
       int?                                         _BufferSize;
       byte[]?                                      _Buffer;
       int?                                         _Memsize;

       //상수 define
       public const int                             _ReadMode = 0;
       public const int                             _WriteMode = 1;

        public void AllocSharedMemory(int MemMode, string MemName = "MemShared",string MutName = "MutName", int MemSize = 4096, int BufferSize = 4096) 
        {
            //외부 파라미터 내부 변수로 복사
            _Mode = MemMode;
            _Memsize = MemSize;
            _BufferSize = BufferSize;
           
            switch (MemMode) 
            {
                case _ReadMode:
                    ReadModeSharedMemory(MemName, MutName, MemSize, BufferSize);
                    break;
                case _WriteMode:
                    writeModeSharedMemory(MemName, MutName, MemSize, BufferSize);
                    break;
            }
        }

        private void ReadModeSharedMemory(string MemName = "MemShared", string MutName = "MutName", int MemSize = 4096, int BufferSize = 4096) 
        {
            bool flag = false;

            //_Mutex = new Mutex(true, MutName, out flag);
            //if (flag == false) return;
            ////초기에 잡고 있는 Mutex Release
            //_Mutex.ReleaseMutex();
            
            //mmf 생성
            _Mmf = MemoryMappedFile.CreateNew(MemName, MemSize);
            if (_Mmf == null) return;

            _Stream = _Mmf.CreateViewStream();
            if (_Stream == null) return;

            _Reader = new BinaryReader(_Stream);
            if (_Reader == null) return;

        }

        private void writeModeSharedMemory(string MemName = "MemShared", string MutName = "MutName", int MemSize = 4096, int BufferSize = 4096)
        {
            _Mmf = MemoryMappedFile.OpenExisting(MemName);
            if (_Mmf == null) return;

            //_Mutex = Mutex.OpenExisting(MutName);
            //if (_Mutex == null) return;

            _Stream = _Mmf.CreateViewStream(1,0);
            if (_Stream == null) return;

            _Writer = new BinaryWriter(_Stream);
            if (_Writer == null) return;
        }

        public void deleteSharedMemory() 
        {
            if (_Mutex != null)
            {
                _Mutex.ReleaseMutex();
                _Mutex.Dispose();
                _Mutex = null;
            }
            
            switch (_Mode)
            {
                case _ReadMode:
                    if (_Reader != null) 
                    {
                        _Reader.Close();
                        _Reader.Dispose();
                        _Reader = null;
                    }
                    break;
                case _WriteMode:
                    if (_Writer != null) 
                    {
                        _Writer.Close();
                        _Writer.Dispose(); 
                        _Writer = null;
                            
                    }
                    break;
            }

            if (_Stream != null) 
            {
                _Stream.Close();
                _Stream.Dispose();
                _Stream = null;
            }

            if (_Mmf != null) 
            {
                _Mmf.Dispose();
                _Mmf = null;
            }
        }

        protected void WriteSharedMemory(string Data) 
        {
            //if (_Mutex == null || _Writer == null) return;
            
            ///_Mutex.WaitOne(); //mutex Lock

            if(Data.Length < _Memsize)
                _Writer.Write(Data);

            //_Mutex.ReleaseMutex(); //mutex UnLock 
        }

        public void ReadSharedMemory(out string Data) 
        {
            //if (_Mutex == null || _Reader == null) 
            //{
            //    Data = "TEST";
            //    return;
            //}

            //_Mutex.WaitOne(); //mutex Lock

            Data = _Reader.ReadString();

            //_Mutex.ReleaseMutex(); //mutex UnLock 

        }
    }

}
