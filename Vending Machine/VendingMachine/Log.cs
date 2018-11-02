using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Log
    {
        private string _destExName = Path.Combine(Environment.CurrentDirectory, @"..\..\..\etc\Exception.txt");
        private string _destPathName = Path.Combine(Environment.CurrentDirectory, @"..\..\..\etc\Log.txt");

        public void FeedMoney(decimal balance, decimal amount)
        {
            WriteLog("Feed Money:", balance, amount);
        }

        //update this to private instead once
        public bool WriteLog(string logDesc, decimal logStartBalance, decimal logEndBalance)
        {
            bool result = false;
            // item description can be up to 18+3 characters
            string logData = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " " +
                         logDesc.PadRight(22) +
                         logStartBalance.ToString("C").PadRight(9) +
                         logEndBalance.ToString("C");
            try
            {
                using (StreamWriter sw = new StreamWriter(_destPathName, true))  // true will append
                {
                    sw.WriteLine(logData);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                string genMessage = "Action failed!  The program encountered an exception writing to the log file.";
                WriteErrorLog(DateTime.Now, genMessage + Environment.NewLine + ex.Message, ex.StackTrace);
            }
            return result;
        }

        public void WriteErrorLog(DateTime date, string message, string stackTrace)
        {
            using (StreamWriter swEx = new StreamWriter(_destExName, true))  // true will append
            {
                swEx.WriteLine(date);
                swEx.WriteLine(message);
                swEx.WriteLine(stackTrace + Environment.NewLine);
            }
        return;
        }
    }
}
