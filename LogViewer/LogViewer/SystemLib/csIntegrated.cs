using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dNetWork;

namespace LogViewer.SystemLib
{
    public class csIntegrated
    {
        csNetWork? _server = null;
        public csNetWork Server { get { return _server; } }

        static private csIntegrated Integrated = null;

        static public csIntegrated Instance
        {
            get
            {
                if (Integrated == null) // 인스턴스가 없으면 생성, 이미 있다면 기존 인스턴스 반환
                {
                    Integrated = new csIntegrated();
                }
                return Integrated;
            }
        }

        private csIntegrated() { }

        public void initIntegratedclass()
        {
            CreateNetWork();
        }

        public void DeleteIntegratedClass()
        {
            if (_server != null) _server.ExitNetWork();
        }

       //내부에서 사용하는 메소드
        bool CreateNetWork() 
        {
            _server = new csNetWork();
            _server.initNetWork(Constant.ServerMode);

            if (_server == null) return false;

            return true;
        }
       
    }
}
