using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// BraList 的摘要说明
    /// </summary>
    public class BraList : IHttpHandler, IRequiresSessionState
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
                DataTable users = SqlHelper.ExecuteDataTable("select * from T_EPC");
                var data = new { Title = "现代科技体验中心", Users = users.Rows, Name = AdminName };
                string html = CommonHelper.RenderHtml("../html/BraList.htm", data);
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