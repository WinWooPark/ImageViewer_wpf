using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dNetwork
{
    static class Constante 
    {
        public const int _Heartbeat                 = 0;
        public const int _Data                      = 1;
        public const int _ACK                       = 2;
        public const int _DataLength                = 3;
        public const int _Exit                      = 4;


        public const int _Server                    = 0;
        public const int _Client                    = 1;

        public const string _sHeartbeat             ="Heartbeat";
        public const string _sData                  ="Data";
        public const string _sACK                   ="ACK";
        public const string _sDataLength            ="DataLength";
        public const string _sExit                  ="Exit";



        //public const int _Heartbeat = 3;
        //public const int _Heartbeat = 4;
        //public const int _Heartbeat = 5;
        //public const int _Heartbeat = 6;
        //public const int _Heartbeat = 7;
        //public const int _Heartbeat = 8;
        //public const int _Heartbeat = 9;
        //public const int _Heartbeat = 10;
    }

    public class csNetWorkPacket
    {
        string      _mode;
        public string Mode { get { return _mode; } set { if (_mode != value) _mode = value; } }

        string      _type;
        public string Type { get { return _type; } set { if (_type != value) _type = value; } }

        string      _data;
        public string Data { get { return _data; } set { if (_data != value) _data = value; } }

        string      _ip;
        public string IP { get { return _ip; } set { if (_ip != value) _ip = value; }}

        int         _port;
        public int Prot { get { return _port; } set { if (_port != value) _port = value; } }

        public static string GetPacketType(int Type) 
        {
            string strType = null;
            switch (Type) 
            {
                case Constante._Heartbeat:
                    strType = Constante._sHeartbeat;
                    break;
                case Constante._Data:
                    strType = Constante._sData;
                    break;
                case Constante._ACK:
                    strType = Constante._sACK;
                    break;
                case Constante._DataLength:
                    strType = Constante._sDataLength;
                    break;
                case Constante._Exit:
                    strType = Constante._sExit;
                    break;
            }
            return strType;
        }

        public static string GetPacketMode(int Mode)
        {
            string strMode = null;
            switch (Mode)
            {
                case Constante._Server:
                    strMode = "Server";
                    break;

                case Constante._Client:
                    strMode = "Client";
                    break;
            }
            return strMode;
        }

       public static string CreateNetWorkPacket(int Mode, string IP, int Port, int Type , string data) 
        {
            csNetWorkPacket obj = new csNetWorkPacket();
            
            obj._mode = GetPacketMode(Mode);
            obj._type = GetPacketType(Type);
            obj._data = data;
            obj._ip = IP;
            obj._port = Port;

            string Packet = JsonConvert.SerializeObject(obj);

            return Packet;
        }

        static bool IsValidJson(string jsonString)
        {
            try
            {
                JToken.Parse(jsonString);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        public static csNetWorkPacket DeserializePacket(string Packet) 
        {
            if (Packet == null) return null;

            bool isValidJson = IsValidJson(Packet);

            if (isValidJson)
            {
                csNetWorkPacket obj = JsonConvert.DeserializeObject<csNetWorkPacket>(Packet);
                return obj;
            }
            else return null;

            
        }


    }
}
