using dNetwork;

using System.Reflection.Metadata;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Logger
{
    class Program
    {
      

        static public void Main()
        {
            csNetLogger Logger = csNetLogger.CreateInstance(dNetWork.Constant.ClientMode);

            Random rand = new Random();
            int Index = 0;

            string[] LogMag = new string[5] { "NetWork Error", "Image Grabb Fali", "Inspction strat", "Image Save Fail", "Image Load Fail" };

            while (true)
            {
                Index = rand.Next(0, 4);
                Logger.Log(dNetWork.csConstant.PriMid, LogMag[Index]);


                Console.WriteLine(LogMag[Index]);
                Thread.Sleep(1000);
            }
        }
    }
    
}