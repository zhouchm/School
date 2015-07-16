using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// UNew 的摘要说明
    /// </summary>
    public class UNew : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string username = (string)context.Session["LoginUserName"];
            if (username == null)
            {
                string str = @"<a href=""/UserLogin.ashx?Action=Log"">登入</a>&nbsp;|&nbsp;<a href=""/UserRegister.ashx?UserReg=Reg"">注册</a>";
                string time = context.Request["time"];
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_News where time=@time", new SqlParameter("@time", time));
                string name = dt.Rows[0]["name"].ToString();
                string title = dt.Rows[0]["title"].ToString();
                string con = dt.Rows[0]["news"].ToString();
                var data = new { Title = "新闻", name, time, title, con, str };
                string html = CommonHelper.RenderHtml("../html/UNew.htm", data);
                context.Response.Write(html);
            }
            else
            {
                string str = "用户:&nbsp;" + username + "&nbsp;欢迎您";
                string time = context.Request["time"];
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_News where time=@time", new SqlParameter("@time", time));
                string name = dt.Rows[0]["name"].ToString();
                string title = dt.Rows[0]["title"].ToString();
                string con = dt.Rows[0]["news"].ToString();
                var data = new { Title = "新闻", name, time, title, con, str };
                string html = CommonHelper.RenderHtml("../html/UNew.htm", data);
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