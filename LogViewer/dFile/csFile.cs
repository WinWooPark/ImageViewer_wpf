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
        string                      _savePath;

        //Folder 관련
        string                      _Foldername;
        string                      _FolderPath; //Path + name

        //File 관련
        string                      _FileName = null;
        string                      _FilePath = null; //Path + name
        System.Threading.Timer      _FolderCreate;

        FileStream                  _FileWriter;

        ConcurrentQueue<string>     _FileWriteData;

        bool                        _IsWriteThreadRun = false;
        Thread                      _WriteThread;

        object                      _FolderNameLock;

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

            //4. File 저장 Thread Run
            _WriteThread = new Thread(Thread_Write);
            _IsWriteThreadRun = true;
            _WriteThread.Start();

            //6. Folder 생성 Timer 시작
            _FolderCreate = new System.Threading.Timer(Timer_CreateFloder,this,0,1000);
        }

       protected void CloseFile() 
        {
            if(_FileWriteData.Count == 0)
                _IsWriteThreadRun = false;
            else 

            _WriteThread.Join();

            _FileWriter.Close();
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

                CreateFileName(Msg);

                string Write = string.Format("{0}\n", Msg);

                //한글로도 로그가 남아야 하기때문에 UTF8로 저장
                byte[] buffer = Encoding.UTF8.GetBytes(Write);

                _FileWriter.Write(buffer);
                _FileWriter.Flush();
                _FileWriter.Seek(0, SeekOrigin.End);

                Thread.Sleep(10);
            }
        }

        void Timer_CreateFloder(object timerState) 
        {
            DateTime now = DateTime.Now;
           
            DateTime midnight = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1);

            TimeSpan timeRemaining = midnight - now;

            if (timeRemaining.TotalMilliseconds <= 0)
                CreateFolderName();
        }

        bool CheckFolder(string Path , bool IsCreate = true)
        {
            if (Directory.Exists(Path) == false)
            {
                if (IsCreate == true) CreateFolderName();
                
                if (Directory.Exists(Path) == false) return false;
            }
            return true;
        }

        bool CheckFile(string Path, bool IsCreate = false)
        {
            if (File.Exists(Path) == false)
            {
                if (IsCreate == true) CreateFile(Path);

                if (File.Exists(Path) == false) return false;
            }
            return true;
        }

        void CreateFolderName()
        {
            string currentTime = DateTime.Now.ToString("yyyy.MM.dd");
            string Foldername = string.Format(currentTime);
            string FolderPath = string.Format("{0}\\{1}", _savePath, _Foldername);

            if (Foldername != _Foldername)
            {
                CreateFolder(Foldername);
            }
        }

        void CreateFolder(string FolderName)
        {
            lock (_FolderNameLock)
            {
                string FolderPath = string.Format("{0}\\{1}", _savePath, FolderName);

                if (Directory.Exists(_FolderPath) == false)
                    Directory.CreateDirectory(_FolderPath);

                _Foldername = FolderName;
                _FolderPath = FolderPath;
            }
            return;
        }


        void CreateFileName(string Name) 
        {
            string Hour = Name.Substring(0, 2);
            string FileName = string.Format("{0}시 LogFile", Hour);

            if (FileName != _FileName) 
            {

                _FileWriter.Close();

                CreateFile(FileName);
            }
        }

        //파일 생성 용도
        void CreateFile(string Path)
        {
            string FilePath = string.Format("{0}\\{1}", _FolderPath, Path);

            try
            {
                if (File.Exists(FilePath) == false) _FileWriter = File.Create(FilePath);

                else _FileWriter = File.Open(FilePath, FileMode.Open);

                _FileName = Path;
                _FilePath = FilePath;

            }
            catch (IOException) 
            {
                return;
            }

            return;
        }

        
    }
}
