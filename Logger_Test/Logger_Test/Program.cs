using dNetwork;

using System.Reflection.Metadata;

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

                Thread.Sleep(1000);
            }
        }
    }
    
}