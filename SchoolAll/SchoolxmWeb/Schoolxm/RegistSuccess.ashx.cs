using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// RegistSuccess 的摘要说明
    /// </summary>
    public class RegistSuccess : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //context.Response.Write("Hello World");
            string userName = context.Request["username"];
            context.Session["LoginUserName"] = userName;
            string str = @"用户:&nbsp;" + userName + "&nbsp;欢迎您";
            string src = "/QRCode/" + userName + ".bmp";
            var data = new { Title = "注册成功", userName, src, str };
            string html = CommonHelper.RenderHtml("../html/RegistSuccess.htm", data);
            context.Response.Write(html);

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