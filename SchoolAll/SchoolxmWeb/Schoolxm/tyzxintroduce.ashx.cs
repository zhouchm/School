using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// tyzxintroduce 的摘要说明
    /// </summary>
    public class tyzxintroduce : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string username = (string)context.Session["LoginUserName"];
            if (username == null)
            {
                context.Response.Redirect("UserLogin.ashx?Action=Log");
            }
            else
            {
                string str = @"用户:&nbsp;" + username + "&nbsp;欢迎您";
                var data = new { Title = "体验导览", str, Name = username };
                string html = CommonHelper.RenderHtml("../html/tyzxintroduce.htm", data);
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