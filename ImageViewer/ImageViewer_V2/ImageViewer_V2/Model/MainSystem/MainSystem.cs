using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer_V2.Model.MainSystem
{
    public class MainSystem
    {
        IntegratedClass _integratedClass;

        public MainSystem() { }

        void InitMainSystem() 
        {
            _integratedClass = IntegratedClass.Instance;
        }

        void ImageLoad() 
        {

        }
    }
}
