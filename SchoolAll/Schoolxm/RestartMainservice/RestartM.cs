using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace RestartMainservice
{
    public partial class RestartM : ServiceBase
    {
        public RestartM()
        {
            InitializeComponent();
        }

        private static int inTimer = 0;
        protected override void OnStart(string[] args)
        {
            this.timer1.Interval = 100;//10S
            this.timer1.Enabled = true;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            this.timer1.Start();
        }

        protected override void OnStop()
        {
            this.timer1.Enabled = false;
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Interlocked.Exchange(ref inTimer, 1) == 0)
            {
                ServiceController service = new ServiceController("MainService");
                
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start(); service.WaitForStatus(ServiceControllerStatus.Running);
                };
                // System.Diagnostics.Process.Start("http://www.baidu.com");
                Interlocked.Exchange(ref inTimer, 0);
            }
        }
    }
}
