using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// UserModPwd 的摘要说明
    /// </summary>
    public class UserModPwd : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string userName = (string)context.Session["LoginUserName"];
            if (userName == null)
            {
                context.Response.Redirect("UserLogin.ashx?Action=Log");
            }
            else
            {
                string str = @"用户:&nbsp;" + userName + "&nbsp;欢迎您";
                var data = new { Title = "修改密码", str, Name = userName };
                string html = CommonHelper.RenderHtml("../html/UserModPwd.htm", data);
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