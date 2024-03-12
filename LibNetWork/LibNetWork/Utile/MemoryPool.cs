using LibNetWork.CommonDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNetWork.Utile
{
    public class MemoryPool
    {
        private static MemoryPool _instance = null;
        public static MemoryPool Instance
        {
            get
            {
                if(_instance == null) _instance = new MemoryPool();

                return _instance;
            }
        }
        private MemoryPool() { }

        int _totalBufferSize;
        byte[] _buffer;

        public void InitMemoryPool(EnumDefine.BufferSize Size) 
        {
            _totalBufferSize = (int)Size;

            _buffer = new byte[_totalBufferSize];
        }



        public void GetBuffer() 
        {

        }


    }
}
