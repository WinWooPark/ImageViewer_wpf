using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer_V2.Model.ManagementSystem;

namespace ImageViewer_V2.ViewModel
{
    public class MainViewModel
    {
        MainSystem _mainSystem;

        public MainViewModel()
        {
            _mainSystem = MainSystem.Instance;
            _mainSystem.IntegratedClass.MainViewModel = this;
        }
    }
}
