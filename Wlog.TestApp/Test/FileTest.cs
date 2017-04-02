using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubLink.TestApp.Test
{
    public class FileTest:TestInstance
    {
        static Logger logger = LogManager.GetLogger("file");

        public override void Execute()
        {
            logger.Info("THIS IS A SAMPLE MESSEGE TO LOG.");
        }
    }
}
