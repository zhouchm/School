using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace CheckStatus
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new Service27() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
