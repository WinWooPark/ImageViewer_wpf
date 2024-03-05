using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Diagnostics.Metrics;

// ����� ���� �޸𸮸� �Ҵ��ϰ� , ����� �а� ���� �ϴ� ������ ���´�.
// string ���� �����͸� �ѱ���̱� ������ ��� ���� �ڽ� class���� ������ �����͸� �Ľ��ؼ� �޸𸮿� ����
// ������ �Ľ̿��� �о���°����� ����.. 

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

       //��� define
       public const int                             _ReadMode = 0;
       public const int                             _WriteMode = 1;

        public void AllocSharedMemory(int MemMode, string MemName = "MemShared",string MutName = "MutName", int MemSize = 4096, int BufferSize = 4096) 
        {
            //�ܺ� �Ķ���� ���� ������ ����
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
            ////�ʱ⿡ ��� �ִ� Mutex Release
            //_Mutex.ReleaseMutex();
            
            //mmf ����
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
