using System.Security.Cryptography.X509Certificates;
using System.Collections.Concurrent;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;

namespace dFile
{
    //비동기 파일 입출력 프로그램
    public class csFile
    {
        string _savePath;

        //Folder 관련
        string _Foldername;
        string _FolderPath; //Path + name

        //File 관련
        string _FileName;
        string _FilePath; //Path + name
        System.Threading.Timer _FolderCreate;

        FileStream FileWriter;

        ConcurrentQueue<string> _FileWriteData;

        bool _IsWriteThreadRun = false;
        Thread _WriteThread;

        object _FolderNameLock;

        protected void InitFile(string Path = "C:\\LogFile")
        {
            //5. Lock 생성
            _FolderNameLock = new object();

            //1. File 저장할 Path 
            if (CheckFolder(Path)) _savePath = Path;
            else return;

            //2. File Queue 초기화
            _FileWriteData = new ConcurrentQueue<string>();
            _FileWriteData.Clear();

            //3. 현재 날짜의 폴더가 존재하는지 확인 존재 하는 폴더가 없으면 오늘 날짜 기준을 파일 생성
            string currentDay = DateTime.Now.ToString("yyyy.MM.dd");
            _Foldername = string.Format(currentDay);
            _FolderPath = string.Format("{0}\\{1}", _savePath, _Foldername);

            if(CheckFolder(_FolderPath) == false) return;


            if (CheckFile(_FolderPath) == false) return;
           

            //4. File 저장 Thread Run
            _WriteThread = new Thread(Thread_Write);
            _IsWriteThreadRun = true;
            _WriteThread.Start();

            

            //6. Folder 생성 Timer 시작
            //_FolderCreate = new System.Threading.Timer();
        }

       protected void CloseFile() 
        {
            _IsWriteThreadRun = false;
            _WriteThread.Join();

            FileWriter.Close();
        }

       protected void AddFileWriteData(string WriteData) 
        {
            _FileWriteData.Enqueue(WriteData);
        }
        void Thread_Write()
        {
            string Msg = null;

            while (_IsWriteThreadRun)
            {
                //Queue가 비어 있을때 예외
                if (_FileWriteData.IsEmpty) continue;

                //Queue가 비어 있을때 예외 처리
                if (_FileWriteData.TryDequeue(out Msg) == false) continue;


                string Write = string.Format("{0}\n", Msg);

                //한글로도 로그가 남아야 하기때문에 UTF8로 저장
                byte[] buffer = Encoding.UTF8.GetBytes(Write);

                FileWriter.Write(buffer);
                FileWriter.Flush();
                FileWriter.Seek(0, SeekOrigin.End);

                Thread.Sleep(10);
            }
        }


        bool CheckFolder(string Path , bool IsCreate = true)
        {
            if (Directory.Exists(Path) == false)
            {
                if (IsCreate == true) CreateFolder();
                
                if (Directory.Exists(Path) == false) return false;
            }
            return true;
        }

        bool CheckFile(string Path, bool IsCreate = true)
        {
            if (File.Exists(Path) == false)
            {
                if (IsCreate == true) CreateFile(Path);

                if (File.Exists(Path) == false) return false;
            }
            return true;
        }

        void CreateFolder()
        {
            lock (_FolderNameLock) 
            {
                //프로그램이 켜질때 해당 폴더가 있는지 없는지 확인하는 작업
                string currentTime = DateTime.Now.ToString("yyyy.MM.dd");
                _Foldername = string.Format(currentTime);
                _FolderPath = string.Format("{0}\\{1}", _savePath, _Foldername);

                if (Directory.Exists(_FolderPath) == false)
                    Directory.CreateDirectory(_FolderPath);
            }
            return;
        }

        void CreateAdvanceFolder()
        {
            lock (_FolderNameLock)
            {
                string currentTime = DateTime.Now.AddDays(1).ToString("yyyy.MM.dd");
                _Foldername = string.Format(currentTime);
                _FolderPath = string.Format("{0}\\{1}", _savePath, _Foldername);

                if (Directory.Exists(_FolderPath) == false)
                    Directory.CreateDirectory(_FolderPath);
            }
            return;
        }

       
        void CreateFile(string Path) 
        {
            string FilePath = string.Format("{0}\\{1}", _FolderPath, Path);

            if (File.Exists(FilePath) == false) FileWriter = File.Create(FilePath);

            else FileWriter = File.Open(FilePath, FileMode.Open);
            
            return;
        }

        
    }
}
