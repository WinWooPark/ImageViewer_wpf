using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageViewer_V2.Model.MainSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer_V2.ViewModel
{
    public class ImageLoadViewModel : ObservableObject
    {
        MainSystem _mainSystem;
        public ImageLoadViewModel()
        {
            _mainSystem = MainSystem.Instance;
            _mainSystem.ViewModels.Add(this.GetType().Name, this);
            CreateCommand();
        }

        public RelayCommand ImageLoad { get; set; }
        public RelayCommand ImageSave { get; set; }
        public RelayCommand ImageChange { get; set; }
        public RelayCommand InspctionStart { get; set; }
        

        void CreateCommand()
        {
            ImageLoad = new RelayCommand(MainSystem.Instance.ImageLoad);
            ImageSave = new RelayCommand(MainSystem.Instance.ImageSave);
            ImageChange = new RelayCommand(MainSystem.Instance.ImageChange);
            InspctionStart = new RelayCommand(MainSystem.Instance.InspctionStart);
        }
    }
}
