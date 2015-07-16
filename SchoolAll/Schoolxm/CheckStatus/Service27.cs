using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data.SqlClient;

namespace CheckStatus
{
    public partial class Service27 : ServiceBase
    {
        public Service27()
        {
            InitializeComponent();
        } private static byte[] result = new byte[1024];
        private static int inTimer = 0;
        protected override void OnStart(string[] args)
        {
            this.timer1.Interval = 500;//10S
            this.timer1.Enabled = true;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            this.timer1.Start();

        }

        protected override void OnStop()
        {
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Interlocked.Exchange(ref inTimer, 1) == 0)
            {
                check();
                // System.Diagnostics.Process.Start("http://www.baidu.com");
                Interlocked.Exchange(ref inTimer, 0);
            }
        }
        public void check(){
            string [] ipa=new string []{"172.16.1.106","172.16.1.114","172.16.1.129","172.16.1.132","172.16.1.140",
            "172.16.1.144","172.16.1.165","172.16.1.166","172.16.1.167","172.16.1.168","172.16.1.172","172.16.1.171"};
            for(int i=0;i<12;i++)
            {
                IPAddress ip = IPAddress.Parse(ipa[i]);
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    if (i == 6 || i == 7 || i == 8 || i == 9)
                        clientSocket.Connect(new IPEndPoint(ip, 7000)); //配置服务器IP与端口  
                    else
                        clientSocket.Connect(new IPEndPoint(ip, 4352)); //配置服务器IP与端口    
                }
                catch
                {
                    
                }
                try
                {
                    string sendMessage;
                    //Thread.Sleep(1000);    //等待1秒钟 
                    if (i == 6 || i == 7 || i == 8 || i == 9)
                        sendMessage = "OP power ?\r";
                    else
                        sendMessage = "%1POWR ?\r";
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));

                }
                catch
                {
                    
                }
                //通过clientSocket接收数据  
                //Thread.Sleep(1);
                string ac, ack = null; string Node_sn;
                if (i == 6 || i == 7 || i == 8 || i == 9)
                {
                    int receiveLength = clientSocket.Receive(result);
                    string rec = Encoding.ASCII.GetString(result, 0, receiveLength);
                    ack = rec.Substring(12, 1); 
                    if (i < 9)
                    {  Node_sn = "PR" + "0" + (i + 1).ToString(); }
                    else {  Node_sn = "PR" + (i + 1).ToString(); }
                    SqlHelper.ExecuteNonQuery(@"update  T_ConNode set Node_Status=@Node_Status where Node_sn=@Node_sn",
                                   new SqlParameter("@Node_Status", ack), new SqlParameter("@Node_sn", Node_sn));
                }
                else
                {
                    int receiveLength = clientSocket.Receive(result);
                    string rec = Encoding.ASCII.GetString(result, 0, receiveLength);
                    ac = rec.Substring(0, 6);
                    if (ac == "%1POWR")
                    {
                        ack = rec.Substring(7, 1);

                    }
                    else
                    {
                        if (rec.Length > 9)
                        {
                            ac = rec.Substring(9, 6);
                            if (ac == "%1POWR")
                                ack = rec.Substring(16, 1);
                            
                            if (i < 9)
                            { Node_sn = "PR" + "0" + (i + 1).ToString(); }
                            else { Node_sn = "PR" + (i + 1).ToString(); }
                            SqlHelper.ExecuteNonQuery(@"update  T_ConNode set Node_Status=@Node_Status where Node_sn=@Node_sn",
                                           new SqlParameter("@Node_Status", ack), new SqlParameter("@Node_sn", Node_sn));
                        }
                        int receiveLength1 = clientSocket.Receive(result);
                        string rec1 = Encoding.ASCII.GetString(result, 0, receiveLength1);
                        ac = rec1.Substring(0, 6);
                        if (ac == "%1POWR")
                            ack = rec1.Substring(7, 1);
                    }
                    
                    if (i < 9)
                    { Node_sn = "PR" + "0" + (i + 1).ToString(); }
                    else { Node_sn = "PR" + (i + 1).ToString(); }
                    SqlHelper.ExecuteNonQuery(@"update  T_ConNode set Node_Status=@Node_Status where Node_sn=@Node_sn",
                                   new SqlParameter("@Node_Status", ack), new SqlParameter("@Node_sn", Node_sn));
                }
            }
            }
           
           
    }
}
