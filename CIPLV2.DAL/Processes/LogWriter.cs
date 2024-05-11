
using System.Reflection;

namespace CIPLV2.DAL.Processes;
public class LogWriter
{
    //private string exePath = string.Empty;
    //public LogWriter(string logMessage)
    //{
    //    LogWrite(logMessage);
    //}
    public static void LogWrite(string logMessage)
    {
        string exePath = "C:\\CIPL\\log";
            //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        try
        {
            using (StreamWriter w = File.AppendText(exePath + "\\" + "log.txt"))
            {
                Log(logMessage, w);
            }
        }
        catch (Exception ex)
        {
        }
    }

    public static void Log(string logMessage, TextWriter txtWriter)
    {
        try
        {
            txtWriter.Write("\r\nLog Entry : ");
            txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            txtWriter.WriteLine("  :");
            txtWriter.WriteLine("  :{0}", logMessage);
            txtWriter.WriteLine("-------------------------------");
        }
        catch (Exception ex)
        {
        }
    }
}
