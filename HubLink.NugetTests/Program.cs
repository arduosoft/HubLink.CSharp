using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using log4net;

namespace HubLink.NugetTests
{
    class Program
    {
        static void Main(string[] args)
        {

            Logger nlogLogger = NLog.LogManager.GetLogger("wlog.logger");
            ILog log4netLogger = log4net.LogManager.GetLogger(typeof(Program));


            for (int i = 0; i < 100; i++)
            {
                nlogLogger.Info("LOG NLOG #" + i);
                log4netLogger.Info("LOG LOG4NET #" + i);
            }
        }
    }
}
