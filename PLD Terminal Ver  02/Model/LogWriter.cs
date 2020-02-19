using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLD_Terminal_Ver__02.Model
{
    class LogWriter
    {
        static public  void WriteLog( string name, string firstRow, string value, int number)
        {

          

            if (number == 1)
            {
                try
                {
                    string path = @"C:\PLD_Log\" + name + ".csv";
                    if (!File.Exists(path))
                    {
                        using (StreamWriter stream = new StreamWriter(path))
                            stream.WriteLine(firstRow, 1);
                    }
                    using (StreamWriter stream = new StreamWriter(path, true))
                        stream.WriteLine(DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year + ";" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ";" + value);
                }
                catch (Exception ex)
                {
                    string path = "ErrorLog_" + Convert.ToString(DateTime.Now.Day) + "_" + Convert.ToString(DateTime.Now.Month) + "_" + Convert.ToString(DateTime.Now.Year) + ".csv";
                    using (StreamWriter stream = new StreamWriter(path, true))
                        stream.WriteLine(DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year + ";" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ";" + ex);
                }
            }
            else
            {
                string path = "ErrorLog_" + Convert.ToString(DateTime.Now.Day) + "_" + Convert.ToString(DateTime.Now.Month) + "_" + Convert.ToString(DateTime.Now.Year) + ".csv";
                using (StreamWriter stream = new StreamWriter(path, true))
                    stream.WriteLine(DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year + ";" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ";" + value);
            }
        }
    }
}
