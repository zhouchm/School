using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// AdminLogin 的摘要说明
    /// </summary>
    public class AdminLogin : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["Action"];
            if (action == "Login")
            {
                string Adminname = context.Request["AdminName"];
                string password = context.Request["Adminpwd"];
                int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Admin where UserName=@UserName and Password=@Password", new SqlParameter("@UserName", Adminname)
                    , new SqlParameter("@Password", password));
                if (count == 1)
                {
                    context.Session["LoginAdminName"] = Adminname;
                    var data = new { Title = "现代科技体验中心", Name = Adminname };
                    string html = CommonHelper.RenderHtml("../html/AdminHomeHavName.htm", data);
                    context.Response.Write(html);
                }
                else if (count > 1)
                {
                    context.Session["LoginAdminName"] = Adminname;
                    var data = new { Title = "现代科技体验中心", Msg = "严重问题，出现用户名重复！" };
                    string html = CommonHelper.RenderHtml("../html/AdminLogin.htm", data);
                    context.Response.Write(html);
                }
                else
                {
                    context.Session["LoginAdminName"] = Adminname;
                    var data = new { Title = "现代科技体验中心", Msg = "用户名或者密码错误！" };
                    string html = CommonHelper.RenderHtml("../html/AdminLogin.htm", data);
                    context.Response.Write(html);
                }
            }
            else if (action == "Cancel") 
            {
                context.Session.Remove("LoginAdminName");
                var data = new { Title = "现代科技体验中心", Msg = "" };
                string html = CommonHelper.RenderHtml("../html/AdminLogin.htm", data);
                context.Response.Write(html);
            }
            else
            {
                var data = new { Title = "现代科技体验中心", Msg = "" };
                string html = CommonHelper.RenderHtml("../html/AdminLogin.htm", data);
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