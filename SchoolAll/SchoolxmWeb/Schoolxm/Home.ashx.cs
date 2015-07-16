using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;

namespace Schoolxm.html
{
    /// <summary>
    /// Home 的摘要说明
    /// </summary>
    public class Home : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string userName = (string)context.Session["LoginUserName"];
            string time = SqlHelper.ExecuteScalar("select VisitTime from T_VisitTime where TID=1").ToString();
            if (userName == null)
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select top 5 * from T_News order by time desc;");
                string str = @"<a href=""/UserLogin.ashx?Action=Log"">登入</a>&nbsp;|&nbsp;<a href=""/UserRegister.ashx?UserReg=Reg"">注册</a>";
                var data = new { Title = "主页", News = dt.Rows, time, str };
                string html = CommonHelper.RenderHtml("../html/HomeNoName.htm", data);
                context.Response.Write(html);
            }
            else
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select top 5 * from T_News order by time desc;");
                string str = @"用户:&nbsp;" + userName + "&nbsp;欢迎您";
                var data = new { Title = "主页", News = dt.Rows, str, time };
                string html = CommonHelper.RenderHtml("../html/HomeHavName.htm", data);
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