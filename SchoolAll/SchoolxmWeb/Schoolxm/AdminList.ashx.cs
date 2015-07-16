using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// AdminList 的摘要说明
    /// </summary>
    public class AdminList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string AdminName = (string)context.Session["LoginAdminName"];
            if (AdminName == null)
            {
                var data = new { Title = "现代科技体验中心", Msg = "" };
                string html = CommonHelper.RenderHtml("../html/AdminLogin.htm", data);
                context.Response.Write(html);
            }
            else
            {
                DataTable admin = SqlHelper.ExecuteDataTable("select * from T_Admin");
                var data = new { Title = "现代科技体验中心", Admin = admin.Rows };
                string html = CommonHelper.RenderHtml("../html/AdminList.htm", data);
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