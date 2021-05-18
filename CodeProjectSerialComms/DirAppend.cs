using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeProjectSerialComms
{
    class DirAppend
    {
        public void LogMessage(string logMessage)
        {
           Label t = Application.OpenForms["Form1"].Controls["lblLogger"] as Label;
            t.Text = logMessage;
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                Log(logMessage, w);
            }

            using (StreamReader r = File.OpenText("log.txt"))
            {
                DumpLog(r);
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()} " + $"  : {logMessage}");
            w.WriteLine("-------------------------------------------------------------------------------------");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
      
    }
}
