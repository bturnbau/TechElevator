using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone.Classes
{   
    public class Log
    {
        /// <summary>
        /// A method to Append each Transaction (deposit(), purchase(), cashout(), error msg) to Log.txt file.
        /// </summary>
        /// <param name="typeOfTransaction">Where it's a purchase, cashout, deposit, or erro</param>
        /// <param name="transacAmnt">Amount to be transacted</param>
        /// <param name="currentbalance">balance after transaction</param>
        public static void AddTransactiontoLog(string typeOfTransaction, double transacAmnt, double currentbalance)
        {
            DateTime now = DateTime.Now;
            string directory = Environment.CurrentDirectory;
            string logFile = "log.txt";
            string logFullPath = Path.Combine(directory, logFile);
            using (StreamWriter sw = new StreamWriter(logFullPath, true))
            {
                sw.WriteLine(String.Format("{0,-1} | {1,-20} | ${2,-5} | ${3, 5}", now, typeOfTransaction, transacAmnt, currentbalance));
            }
        }
    }
}

