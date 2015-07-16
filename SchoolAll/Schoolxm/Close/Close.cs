using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Close
{
    public partial class Close : ServiceBase
    {
        public Close()
        {
            InitializeComponent();
           
        }
        static Socket serverSocket;
        private static int myProt = 8885;   //端口 
        private static byte[] result = new byte[1024];
        protected override void OnStart(string[] args)
        {
            string HostName = Dns.GetHostName();
            IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
            IPAddress ip = null;
            for (int i = 0; i < IpEntry.AddressList.Length; i++)
            {
                if (IpEntry.AddressList[i].AddressFamily.ToString() == "InterNetwork")
                {
                    ip = IpEntry.AddressList[i];
                    break;
                }
            }
            //IPAddress ip = IPAddress.Parse("172.16.1.115");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
            serverSocket.Listen(10);    //设定最多10个排队连接请求 
            Thread myThread = new Thread(ListenClientConnect);
            myThread.IsBackground = true;
            myThread.Start();
           
        }
        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
               //clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello"));
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.IsBackground = true;
                receiveThread.Start(clientSocket);
            }
        }
        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    //通过clientSocket接收数据  
                    int receiveNumber = myClientSocket.Receive(result);
                    string message = Encoding.ASCII.GetString(result, 0, receiveNumber);
                    //Console.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), message);
                    if (message.Equals("g"))
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = "cmd.exe";
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardInput = true;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.CreateNoWindow = false;
                        p.Start();
                        p.StandardInput.WriteLine("shutdown -s -t 0 -f");
                        p.StandardInput.WriteLine("exit");
                        p.WaitForExit();
                        p.Close();

                    }
                    if (message.Equals("k"))
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = "cmd.exe";
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardInput = true;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.CreateNoWindow = false;
                        p.Start();
                        p.StandardInput.WriteLine("shutdown -s");
                        p.StandardInput.WriteLine("exit");
                        p.WaitForExit();
                        p.Close();

                    }
                }
                catch (Exception ex)
                {
                   // Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }
        protected override void OnStop()
        {
        }
    }
}
