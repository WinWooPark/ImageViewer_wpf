using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Reflection;
using LogViewer.ViewModel;


namespace LogViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewerModel model;
       
        public MainWindow()
        {
            InitializeComponent();
            model = new MainViewerModel();
            this.DataContext = model; //연결

            //ListView.ScrollIntoView(model.LogData[model.LogData.Count - 1]);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 종료 전에 수행할 작업을 여기에 추가
            MessageBoxResult result = MessageBox.Show("종료하시겠습니까?", "확인", MessageBoxButton.YesNo, MessageBoxImage.Question);
            


            if (result == MessageBoxResult.No)
            {
                // 사용자가 종료를 취소하면 종료를 취소하고 이벤트를 취소
                e.Cancel = true;
            }
            else
            {
                model.CloseViewModel();
                // 사용자가 종료를 선택하면 정상 종료
                // 여기에 파일 저장, 설정 저장 등의 작업을 추가할 수 있습니다.
            }
        }

    }

   

}