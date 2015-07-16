using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web.SessionState;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// UserRegister 的摘要说明
    /// </summary>
    public class UserRegister : IHttpHandler, IRequiresSessionState
    {
        protected string UrlEncode(string url)
        {
            byte[] bs = Encoding.GetEncoding("GB2312").GetBytes(url);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                if (bs[i] < 128)
                    sb.Append((char)bs[i]);
                else
                {
                    sb.Append("%" + bs[i++].ToString("x").PadLeft(2, '0'));
                    sb.Append("%" + bs[i].ToString("x").PadLeft(2, '0'));
                }
            }
            return sb.ToString();
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string UserReg = context.Request["UserReg"];
            if (UserReg == "0")
            {
                User user = new User();
                user.UserName = context.Request["userName"];
                user.Password = context.Request["userpwd"];
                user.Email = context.Request["email"];
                user.Gender = context.Request["sex"];
                user.RealName = context.Request["RealName"];

                string url = "http://tyzx.sesedu.cn/UserRegister.ashx?UserReg=1&userName=";
                url = url + user.UserName + "&amp;userpwd=";
                url = url + user.Password + "&amp;email=";
                url = url + user.Email + "&amp;sex=";
                url = url + user.Gender + "&amp;RealName=";
                url = url + HttpUtility.UrlEncode(user.RealName) + "&amp;time=";
                url = url + DateTime.Now;
                var data = new { Email = user.Email, url };
                string html = CommonHelper.RenderHtml("../html/RegisterEmail.htm", data);
                string from = "ses_tyzx@163.com";//发件人邮箱
                string fromer = "上海市实验学校";
                string to = user.Email;
                string toer = user.Email;//收件人
                string Subject = "实验学校注册";
                string file = "";
                string Body = html;
                string SMTPHost = "smtp.163.com";//发件人邮箱服务器
                string SMTPuser = "kingofcrane@163.com";//发件人邮箱账户
                string SMTPpass = "tyzx1234";//发件人邮箱密码
                sendmail(from, fromer, to, toer, Subject, Body, file, SMTPHost, SMTPuser, SMTPpass);
                string str = @"<a href=""/UserLogin.ashx?Action=Log"">登入</a>&nbsp;|&nbsp;<a href=""/UserRegister.ashx?UserReg=Reg"">注册</a>";
                String msg = "恭喜，现在只差一步，登录邮箱激活账号便可以完成注册！";
                var data1 = new { Title = "用户注册", Msg = msg, str };
                string html1 = CommonHelper.RenderHtml("../html/RegistPreSuccess.htm", data1);
                context.Response.Write(html1);
            }
            else if (UserReg == "1")
            {

                User user = new User();

                string userName = context.Request["UserName"];
                int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Users where UserName=@UserName", new SqlParameter("@UserName", userName));
                if (count <= 0)
                {
                    user.UserName = context.Request["userName"];
                    user.Password = context.Request["userpwd"];
                    user.Email = context.Request["email"];
                    user.Gender = context.Request["sex"];
                    user.RealName = HttpUtility.UrlDecode(context.Request["RealName"]);
                    DateTime old = Convert.ToDateTime(context.Request["time"]);
                    System.TimeSpan NowValue = new TimeSpan(DateTime.Now.Ticks);
                    System.TimeSpan TimeValue = new TimeSpan(old.Ticks);
                    System.TimeSpan DateDiff = TimeSpan.Zero;
                    DateDiff = TimeValue.Subtract(NowValue);
                    int hours = DateDiff.Hours;
                    int minutes = DateDiff.Minutes;
                    int seconds = DateDiff.Seconds;
                    int lReturn = hours * 3600 * 1000
                        + minutes * 60 * 1000
                        + seconds;
                    if (lReturn <= 1800)
                    {
                        UserDAL.Insert(user);
                        //context.Session["LoginUserName"] = context.Request["userName"];
                        CreateQRCode(context.Request["userName"]);
                        string url = "RegistSuccess.ashx?username=" + user.UserName;
                        context.Response.Redirect(url);//重定向到注册成功页面
                    }
                    else
                    {
                        string str = @"<a href=""/UserLogin.ashx?Action=Log"">登入</a>&nbsp;|&nbsp;<a href=""/UserRegister.ashx?UserReg=Reg"">注册</a>";
                        var data = new { Title = "用户注册", Msg = "此链接已经失效,请重新注册!", str };
                        string html = CommonHelper.RenderHtml("../html/UserRegister.htm", data);
                        context.Response.Write(html);
                    }

                }
                else
                {
                    string str = @"<a href=""/UserLogin.ashx?Action=Log"">登入</a>&nbsp;|&nbsp;<a href=""/UserRegister.ashx?UserReg=Reg"">注册</a>";
                    var data = new { Title = "用户注册", Msg = "此账号已经存在,请勿重复点击!", str };
                    string html = CommonHelper.RenderHtml("../html/UserRegister.htm", data);
                    context.Response.Write(html);
                }

            }
            else if (UserReg == "Reg")
            {
                string str = @"<a href=""/UserLogin.ashx?Action=Log"">登入</a>&nbsp;|&nbsp;<a href=""/UserRegister.ashx?UserReg=Reg"">注册</a>";
                var data = new { Title = "用户注册", Msg = "", str };
                string html = CommonHelper.RenderHtml("../html/UserRegister.htm", data);
                context.Response.Write(html);
            }
        }
        public void CreateQRCode(string userName)
        {
            Bitmap bt;
            string enCodeString = userName;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            //string filename = string.Format(DateTime.Now.ToString(), "yyyymmddhhmmss");
            //filename = filename.Replace(" ", "");
            //filename = filename.Replace(":", "");
            //filename = filename.Replace("-", "");
            //filename = filename.Replace(".", "");
            //filename = filename + userName;
            bt.Save(System.Web.HttpContext.Current.Server.MapPath("~/QRCode/") + userName + ".bmp");
            //this.Image1.ImageUrl = "~/Images/" + filename + ".jpg";
        }
        /// <summary>
        /// C#发送邮件函数
        /// </summary>
        /// <param name="from">发送者邮箱</param>
        /// <param name="fromer">发送人</param>
        /// <param name="to">接受者邮箱</param>
        /// <param name="toer">收件人</param>
        /// <param name="Subject">主题</param>
        /// <param name="Body">内容</param>
        /// <param name="file">附件</param>
        /// <param name="SMTPHost">smtp服务器</param>
        /// <param name="SMTPuser">邮箱</param>
        /// <param name="SMTPpass">密码</param>

        /// <returns></returns>
        public bool sendmail(string sfrom, string sfromer, string sto, string stoer, string sSubject, string sBody, string sfile, string sSMTPHost, string sSMTPuser, string sSMTPpass)
        {
            ////设置from和to地址
            MailAddress from = new MailAddress(sfrom, sfromer);
            MailAddress to = new MailAddress(sto, stoer);

            ////创建一个MailMessage对象
            MailMessage oMail = new MailMessage(from, to);

            //// 添加附件
            if (sfile != "")
            {
                oMail.Attachments.Add(new Attachment(sfile));
            }



            ////邮件标题
            oMail.Subject = sSubject;


            ////邮件内容
            oMail.Body = sBody;

            ////邮件格式
            oMail.IsBodyHtml = true;

            ////邮件采用的编码
            oMail.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");

            ////设置邮件的优先级为高
            oMail.Priority = MailPriority.High;

            ////发送邮件
            SmtpClient client = new SmtpClient();
            ////client.UseDefaultCredentials = false; 
            client.Host = sSMTPHost;
            client.Credentials = new NetworkCredential(sSMTPuser, sSMTPpass);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(oMail);
                return true;
            }
            catch (Exception err)
            {
                //Response.Write(err.Message.ToString());
                return false;
            }
            finally
            {
                ////释放资源
                oMail.Dispose();
            }

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}