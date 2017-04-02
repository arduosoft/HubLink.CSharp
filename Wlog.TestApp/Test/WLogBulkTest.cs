using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubLink.TestApp.Test
{
    public class WLogBulkTest : TestInstance
    {
        static Logger logger = LogManager.GetLogger("HubLink.loggerWithBuffer");

        public override void Execute()
        {
            logger.Info("THIS IS A SAMPLE MESSEGE TO LOG.");
        }
    }
}
