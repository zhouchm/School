using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketCSClient
{
    class Program
    {
        private static byte[] result = new byte[1024];  
        static void Main(string[] args)
        {//设定服务器IP地址  
            IPAddress ip = IPAddress.Parse("172.16.1.171");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 4352)); //配置服务器IP与端口  
                Console.WriteLine("连接服务器成功");
            }
            catch
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
                return;
            }
           
            //通过 clientSocket 发送数据  
            for (int i = 0; i < 1; i++)
            {
                try
                {
                    Thread.Sleep(1000);    //等待1秒钟  
                    string sendMessage = "%1POWR ?\r"; string sendMessage1 = "OP power ?\r";
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                    Console.WriteLine("向服务器发送消息：{0}" + sendMessage);
                }
                catch
                {
                    //clientSocket.Shutdown(SocketShutdown.Both);
                    //clientSocket.Close();
                    //break;
                }

                //通过clientSocket接收数据  
                //Thread.Sleep(3);
                int receiveLength = clientSocket.Receive(result);
                Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength));
                int receiveLength1 = clientSocket.Receive(result);
                Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength1));
                string ac, ack = null;

                string rec = Encoding.ASCII.GetString(result, 0, receiveLength1);
                ac = rec.Substring(0, 6);
                if (ac == "%1POWR")
                {
                    ack = rec.Substring(7, 1);

                }
                //else
                //{
                //    if (rec.Length > 9)
                //    {
                //        ac = rec.Substring(9, 6);
                //        if (ac == "%1POWR")
                //            ack = rec.Substring(16, 1);
                //        return ack;
                //    }
                //    int receiveLength1 = clientSocket.Receive(result);
                //    string rec1 = Encoding.ASCII.GetString(result, 0, receiveLength1);
                //    ac = rec1.Substring(0, 6);
                //    if (ac == "%1POWR")
                //        ack = rec1.Substring(7, 1);
                //}
                //return ack;

                Console.WriteLine("状态"+ack);
                Console.ReadLine();
            }
        } 
    }
}
