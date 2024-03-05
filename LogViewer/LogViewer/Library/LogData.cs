using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;


//여기에서 XML 데이터를 만들자.
namespace LogViewer.Library
{

    public class LogData
    {
        public LogData() { }

        public static string CreateLogData(string IP, int Priority, string Msg, int FuncStep = 2)
        {
            string strDate = GetCurrentTime24Hour();
            string strFunc = GetPreFuntionName(FuncStep);
            string strPriority = GetPriority(Priority);
            string strLog_Msg = Msg;

            XmlDocument Xml = new XmlDocument();
            XmlElement Root = Xml.CreateElement("LogData");
            Xml.AppendChild(Root);

            XmlElement Element = Xml.CreateElement("IP");
            Element.InnerText = IP;
            Root.AppendChild(Element);

            Element = Xml.CreateElement(csConstant.coDate);
            Element.InnerText = strDate;
            Root.AppendChild(Element);

            Element = Xml.CreateElement(csConstant.coFunc);
            Element.InnerText = strDate;
            Root.AppendChild(Element);

            Element = Xml.CreateElement(csConstant.coPriority);
            Element.InnerText = strPriority;
            Root.AppendChild(Element);

            Element = Xml.CreateElement(csConstant.coLogMsg);
            Element.InnerText = strLog_Msg;
            Root.AppendChild(Element);

            string xmlString = Root.OuterXml;

            string SendMsg = string.Join(" ", strDate, strFunc, strPriority, strLog_Msg);
            return xmlString;
        }

        public static string GetCurrentTime24Hour()
        {
            DateTime currentTime = DateTime.Now;
            string currentTime24Hour = currentTime.ToString("HH:mm:ss");

            return currentTime24Hour;
        }

        public static string GetPreFuntionName(int Step = 2)
        {
            string FuncName = new StackFrame(Step, true).GetMethod().Name;
            return FuncName;
        }

        public static string GetPriority(int Priority) 
        {
            string Pri = "MID";

            switch (Priority) 
            {
                case csConstant.PriHig:
                    Pri = "HIGHT";
                    break;

                case csConstant.PriMid:
                    Pri = "MID";
                    break;

                case csConstant.PriLow:
                    Pri = "LOW";
                    break;

                default:
                    Pri = "MID";
                    break;
            }
            return Pri;
        }
    }
}
