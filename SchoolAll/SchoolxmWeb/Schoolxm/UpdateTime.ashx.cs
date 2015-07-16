using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// UpdateTime 的摘要说明
    /// </summary>
    public class UpdateTime : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string message = null;
            string AdminName = (string)context.Session["LoginAdminName"];
            if (AdminName == null)
            {
                var data = new { Title = "现代科技体验中心", Msg = "" };
                string html = CommonHelper.RenderHtml("../html/AdminLogin.htm", data);
                context.Response.Write(html);
                return;
            }
            string grade=context.Request["grade"];
            string Class=context.Request["Class"];
            string Day=context.Request["day"];
            string Time=context.Request["time"];
            int count = Convert.ToInt32(context.Request["Count"]);//已经预约的人数
            string name = context.Request["checkbox"];//添加的学生的名字
            string[] word = null;
            try
            {
                word = name.Split(new Char[] { ' ', ',' });//字符串拆分
            }
            catch(Exception e)
            {
                var data = new { Msg = "首先请选择班级和需要预约的学生，再进行预约操作！", Name = AdminName };
                string html = CommonHelper.RenderHtml("../html/GroupOrderMessage.htm", data);
                context.Response.Write(html);
                return;
            }
            int CheckedNum = word.Length;//选择学生的个数
            if ((count + CheckedNum) <= 50)
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select UserName from T_TimeStudent where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP"
                    , new SqlParameter[] { new SqlParameter("@TimeOfDay", Day), new SqlParameter("@TimeOfAP", Time)});
                for (int i = 0; i < CheckedNum; i++)
                {
                    Boolean chongFu = false;
                    string str = word[i];
                    int Num = Convert.ToInt32(dt.Rows.Count);
                    for (int j = 0; j < Num; j++)
                    {
                        if (str.Equals(dt.Rows[j]["UserName"]))
                        {
                            chongFu = true;
                            break;
                        }
                    }
                    if (chongFu == true)
                    {
                        message = message + str + "已经预约！";
                        if (i == (CheckedNum - 1))
                        {
                            var data = new { Msg = message, Name = AdminName };
                            string html = CommonHelper.RenderHtml("../html/GroupOrderMessage.htm", data);
                            context.Response.Write(html);
                        }
                        continue;
                    }
                    else
                    {
                        if (i == (CheckedNum - 1))
                        {
                            message = message + str + "预约成功！";
                        }
                        else
                        {
                            message = message + str + "、";
                        }
                        if (i == (CheckedNum - 1))
                        {
                            var data = new { Msg = message, Name = AdminName };
                            string html = CommonHelper.RenderHtml("../html/GroupOrderMessage.htm", data);
                            context.Response.Write(html);
                        }
                        SqlHelper.ExecuteNonQuery("insert into T_TimeStudent values(@TimeOfDay,@TimeOfAP,@UserName)"
                            , new SqlParameter[] { new SqlParameter("@TimeOfDay", Day), 
                            new SqlParameter("@TimeOfAP", Time), new SqlParameter("@UserName", str)});
                        if (count == 0)
                        {
                            SqlHelper.ExecuteNonQuery("insert into T_TimeNumber values(@TimeOfDay,@TimeOfAP,@OrderUser)"
                                , new SqlParameter[] { new SqlParameter("@TimeOfDay", Day), 
                                new SqlParameter("@TimeOfAP", Time), new SqlParameter("@OrderUser", CheckedNum)});
                        }
                        else
                        {
                            SqlHelper.ExecuteNonQuery("update T_TimeNumber set OrderUser=OrderUser+1 where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP"
                                , new SqlParameter[] { new SqlParameter("@TimeOfDay", Day), new SqlParameter("@TimeOfAP", Time) });
                        }
                    }
                }
            }
            else {
                int num = 50 - count;
                var data = new { Msg = "本次预约人数不能超过" + num + "人，请重新操作！", Name = AdminName };
                string html = CommonHelper.RenderHtml("../html/GroupOrderMessage.htm", data);
                context.Response.Write(html);
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