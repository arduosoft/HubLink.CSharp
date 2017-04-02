using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HubLink.TestApp.Test;
using log4net;

namespace HubLink.TestApp
{
    class Program
    {
        static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                Console.WriteLine("Making a single call to service to check availability...");
                //Manual call to log service to test plain performance
                DateTime d1 = DateTime.Now;
             
                double ms = DateTime.Now.Subtract(d1).TotalMilliseconds;
                Console.WriteLine("Service call done in ms:" + ms);

                Logger nlogLogger = NLog.LogManager.GetLogger("wlog.logger");
                ILog log4netLogger = log4net.LogManager.GetLogger(typeof(Program));


                for (int i = 0; i < 100; i++)
                {
                  //nlogLogger.Info("LOG NLOG #" + i);
                    log4netLogger.Info("LOG LOG4NET #" + i);
                }


                //Performance test
                TestIterator it = new TestIterator();
                it.Instances.Add(new WlogTest());
                it.Instances.Add(new WLogBulkTest());
                it.Instances.Add(new FileTest());

                int[] iterationSize = new int[] { 1, 10, 100, 1000, 10000 };
                Console.WriteLine("#;Wlog;Wlog (bulk);File;");
                for (int i = 0; i < iterationSize.Length; i++)
                {
                    it.RepeatCount = iterationSize[i];
                    //Console.WriteLine("=> Starting benchmark with iterationSize " + it.RepeatCount);

                    it.DoTest();

                    Console.WriteLine("{0};{1};{2};{3}", it.RepeatCount, it.Instances[0].Avg, it.Instances[1].Avg, it.Instances[2].Avg);
                }
            }
            catch (Exception er)
            {
                Console.WriteLine(er.StackTrace);
            }


            Console.ReadKey();
        }
    }
}
