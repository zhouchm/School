using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Schoolxm
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //string userName = context.Request["UserName"];
            DateTime now = DateTime.Now;
            string Time;
            string time = now.Hour.ToString();
            if (Convert.ToInt32(time) > 12)
            { Time = "下午"; }
            else { Time = "上午"; }
            string Day = now.ToString("yyyy年MM月dd日");
            string check = context.Request["check"];
            string username = context.Request["UserName"];
            if (check == "0")
            {
                int count=0;
                int count1 =  (int)SqlHelper.ExecuteScalar("select count(*) from T_Student where StuName=@UserName ", new SqlParameter("@UserName", username));
                if (count1 == 0)
                {
                    string password = context.Request["userpwd"];

                   count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Users where UserName=@UserName and Password=@Password", new SqlParameter("@UserName", username)
                        , new SqlParameter("@Password", password));
                   
                }
                DataTable dt=null;
                if (count == 1)
                {
                     dt = SqlHelper.ExecuteDataTable("select * from T_TimeUser where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP and UserName=@UserName"
                    , new SqlParameter[] { new SqlParameter("@TimeOfDay", Day), 
                                new SqlParameter("@TimeOfAP", Time),
                                new SqlParameter("@UserName",username)});
                }else if(count1==1)
                {  dt = SqlHelper.ExecuteDataTable("select * from T_TimeStudent where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP and UserName=@UserName"
                    , new SqlParameter[] { new SqlParameter("@TimeOfDay", Day), 
                                new SqlParameter("@TimeOfAP", Time),
                                new SqlParameter("@UserName",username)});}
                if(count==1||count1==1){
                if (dt.Rows.Count <= 0)
                    {

                        var data = new { swfur = "/Images/Geterror.png", Msg = "请在预约时间段内访问!" };
                        string html = CommonHelper.RenderHtml("../html/GetSuccess.htm", data);
                        context.Response.Write(html);
                    }
                    else if (dt.Rows.Count > 0)
                    {
                        DataTable a = SqlHelper.ExecuteDataTable("select * from T_UserRFID where UserName=@UserName ", new SqlParameter("@UserName", username));
                        if (a.Rows.Count == 0)
                        {
                            SqlHelper.ExecuteNonQuery(@"insert into T_UserRFID(UserName,udate) values(@username,@time)",
                                new SqlParameter("@username", username), new SqlParameter("@time", DateTime.Now.ToString()));
                            var data = new { swfur = "/Images/Getsuccess.png", Msg = "请取手环!" }; string html = CommonHelper.RenderHtml("../html/GetSuccess.htm", data);
                            context.Response.Write(html);
                        }
                        else
                        {
                            var data1 = new { swfur = "/Images/Geterror.png", Msg = "此账号已经在参观!" };
                            string html = CommonHelper.RenderHtml("../html/GetSuccess.htm", data1); context.Response.Write(html);
                        }

                    }
                }
                else
                {
                    var data = new { swfur = "/Images/Geterror.png", Msg = "账户名或密码错误!" };
                    string html = CommonHelper.RenderHtml("../html/GetSuccess.htm", data);
                    context.Response.Write(html);
                }
            }
            else if (check == "1")
            {
                int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Users where UserName=@UserName", new SqlParameter("@UserName", username)
                    );
                if (count == 1)
                {
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_TimeUser where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP and UserName=@UserName"
                    , new SqlParameter[] { new SqlParameter("@TimeOfDay", Day), 
                                new SqlParameter("@TimeOfAP", Time),
                                new SqlParameter("@UserName",username)});
                    if (dt.Rows.Count <= 0)
                    {

                        var data = new { swfur = "/Images/Geterror.png", Msg = "请在预约时间段内访问!" };
                        string html = CommonHelper.RenderHtml("../html/GetSuccess.htm", data);
                        context.Response.Write(html);
                    }
                    else if (dt.Rows.Count > 0)
                    {
                        DataTable a = SqlHelper.ExecuteDataTable("select * from T_UserRFID where UserName=@UserName ", new SqlParameter("@UserName", username));
                        if (a.Rows.Count == 0)
                        {
                            SqlHelper.ExecuteNonQuery(@"insert into T_UserRFID(UserName,RFID_STATUS,udate) values(@username,@RFID_STATUS,@time)",
                                new SqlParameter("@username", username), new SqlParameter("@RFID_STATUS", "0"), new SqlParameter("@time", DateTime.Now.ToString()));
                            var data = new { swfur = "/Images/Getsuccess.png", Msg = "请取手环!" }; string html = CommonHelper.RenderHtml("../html/GetSuccess.htm", data);
                            context.Response.Write(html);
                        }
                        else
                        {
                            var data1 = new { swfur = "/Images/Geterror.png", Msg = "此账号已经在参观!" };
                            string html = CommonHelper.RenderHtml("../html/GetSuccess.htm", data1); context.Response.Write(html);
                        }

                    }
                }
                else
                {
                    var data = new { swfur = "/Images/Geterror.png", Msg = "二维码信息错误!" };
                    string html = CommonHelper.RenderHtml("../html/GetSuccess.htm", data);
                    context.Response.Write(html);
                }
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