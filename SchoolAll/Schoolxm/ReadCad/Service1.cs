using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using ReaderB;
using System.Data.SqlClient;
using System.Threading;
using System.Runtime.InteropServices;
using System.Timers;
namespace ReadCad
{
    public partial class Service1 : ServiceBase
    {
         int port;
        string IPAddr;
        int fCmdRet = 30; //所有执行指令的返回值
        int frmcomportindex = 0;
        byte fComAdr;
        byte WordPtr, ENum;
        byte Num = 0;
        byte EPClength = 0;
        byte[] fPassWord = new byte[4];
        byte Maskadr;
        byte MaskLen;
        byte MaskFlag;
        byte[] CardData = new byte[320];
        byte[] CardData1 = new byte[320];
        int ferrorcode = 0;
        private static int inTimer = 0;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            fComAdr = Convert.ToByte("02", 16); // $FF;
            port = Convert.ToInt32("6000");
            IPAddr = "172.16.1.105";
            MaskFlag = 0;
            Maskadr = Convert.ToByte("00", 16);
            MaskLen = Convert.ToByte("00", 16);
            WordPtr = Convert.ToByte("00", 16);
            Num = Convert.ToByte("4");
            fPassWord = HexStringToByteArray("00000000");
            StaticClassReaderB.OpenNetPort(port, IPAddr, ref fComAdr, ref frmcomportindex);
            this.timer1.Interval = 500;//10S
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
                ReadCard();
                // System.Diagnostics.Process.Start("http://www.baidu.com");
                Interlocked.Exchange(ref inTimer, 0);
            }
        }
        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();

        }
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
        public void ReadCard()
        {
            int CardNum = 0;
            int Totallen = 0;
            int EPClen, m;
            byte[] EPC = new byte[5000];
            int CardIndex;
            string temps;
            string s, sEPC;

            //fIsInventoryScan = true;
            byte AdrTID = 0;
            byte LenTID = 0;
            byte TIDFlag = 0;
            fCmdRet = StaticClassReaderB.Inventory_G2(ref fComAdr, AdrTID, LenTID, TIDFlag, EPC, ref Totallen, ref CardNum, frmcomportindex);
            if ((fCmdRet == 1) | (fCmdRet == 2) | (fCmdRet == 3) | (fCmdRet == 4) | (fCmdRet == 0xFB))//代表已查找结束，
            {
                byte[] daw = new byte[Totallen];
                Array.Copy(EPC, daw, Totallen); //把EPC数组拷贝到daw数组中              
                temps = ByteArrayToHexString(daw);
                m = 0;
                if (CardNum == 0)
                {

                    return;
                }
                for (CardIndex = 0; CardIndex < CardNum; CardIndex++)
                {
                    EPClen = daw[m];
                    sEPC = temps.Substring(m * 2 + 2, EPClen * 2);
                    m = m + EPClen + 1;
                    if (sEPC.Length != EPClen * 2)
                        return;

                    string flags = SqlHelper.ExecuteScalar("select Node_Open from T_NodeOpe where  Node_sn=@Node_sn", new SqlParameter("@Node_sn", "02")).ToString();
                    if (flags != "1")//*************************************Node_Ope 存放的是节点近ID开关表
                    {
                        SqlHelper.ExecuteNonQuery(@"update  T_NodeOpe set Node_Open=1 where Node_sn=@Node_sn",
                                   new SqlParameter("@Node_sn", "02"));
                       
                            SqlHelper.ExecuteNonQuery(@"update T_UserRFID set RFID10=@RFID10,RFID_STATUS=@RFID_STATUS where RFID_STATUS='0'",
                                new SqlParameter("@RFID10", sEPC), new SqlParameter("@RFID_STATUS", "1"));
                            SqlHelper.ExecuteNonQuery(@"update  T_NodeOpe set Node_Open=0 where Node_sn=@Node_sn",
                                       new SqlParameter("@Node_sn", "02"));
                      
                    }
                }
                    }
                }
            }
        }

