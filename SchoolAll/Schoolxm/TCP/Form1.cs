using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TCP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TcpClient tcpClient = null;
        BinaryWriter writer = null;
        private void button1_Click(object sender, EventArgs e)
        {
            Thread connectThread = new Thread(ConnectToServer);
            connectThread.Start();
        }
        private void ConnectToServer()
        {
            try
            {
                // 调用委托

                NetworkStream networkStream = null;
                IPAddress ipaddress = IPAddress.Parse("172.16.1.105");
                tcpClient = new TcpClient();
                tcpClient.Connect(ipaddress, int.Parse("6000"));

                // 延时操作
                //Thread.Sleep(1000);
                if (tcpClient != null)
                {
                   // statusStripInfo.Invoke(showStatusCallBack, "连接成功");
                    networkStream = tcpClient.GetStream();
                   BinaryReader reader = new BinaryReader(networkStream);
                   writer = new BinaryWriter(networkStream);
                }


            }
            catch
            {
                //statusStripInfo.Invoke(showStatusCallBack, "连接失败");
               // Thread.Sleep(1000);
                //statusStripInfo.Invoke(showStatusCallBack, "就绪");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread sendThread = new Thread(SendMessage);
            sendThread.Start("op power.on");//要不要加回车字符 CR 0Dh    : PWR ON CR
        }
        private void SendMessage(object state)
        {
            
            //statusStripInfo.Invoke(showStatusCallBack, "正在发送...");
            try
            {
                writer.Write(state.ToString());
                //Thread.Sleep(5000);
                writer.Flush();
                //statusStripInfo.Invoke(showStatusCallBack, "完毕");

               // tbxMessage.Invoke(resetMessageCallBack, null);
                //lstbxMessageView.Invoke(showMessageCallback, state.ToString());
            }
            catch
            {
                //if (reader != null)
                //{
                //    reader.Close();
                //}
                if (writer != null)
                {
                    writer.Close();
                }
                if (tcpClient != null)
                {
                    tcpClient.Close();
                }

                //statusStripInfo.Invoke(showStatusCallBack, "断开了连接");
            }
        }
    }
}
