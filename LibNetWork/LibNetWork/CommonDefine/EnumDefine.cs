
namespace LibNetWork.CommonDefine
{
    public static class EnumDefine
    {
        public enum TCPIPMode
        {
            eHost = 0,
            eClient = 1
        }
        public enum BufferSize
        {
            eByte128 = 128,
            eByte256 = 256,
            eByte512 = 512,
            eByte1024 = 1024,
            eByte2048 = 2048,
            eByte4096 = 4096,
            eByte8192 = 8192
        }

        public enum PacketType
        {
            eHeartBeat = 0,
            eDataSend = 1,
            eDataLength = 2
        }
    }
}
