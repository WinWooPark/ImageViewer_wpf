using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dFile;
using dNetWork;

namespace LogViewer.SystemLib
{
    public class FileManager : csFile
    {
        public void InitFileManager(string Path = "C:\\LogFile") 
        {
            InitFile(Path);

        }

        public void MakeLogDataForFileDataAndAddFileData(dNetwork.LogData Log) 
        {
            string WriteFile = null;

            string[] arr = { Log.Date, Log.Func, Log.Priority, Log.Log_Msg };

            WriteFile = string.Join(" ", arr);
            
            AddFileWriteData(WriteFile);

        }
        public void ExitFileManager() 
        {
            CloseFile();
        }
    }
}
