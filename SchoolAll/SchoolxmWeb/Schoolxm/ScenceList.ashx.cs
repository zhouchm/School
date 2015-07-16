using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace Schoolxm
{

    public class ScenceList : IHttpHandler, IRequiresSessionState
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
                DataTable scence = SqlHelper.ExecuteDataTable("select * from T_Scence");
                var data = new { Title = "现代科技体验中心", Scence = scence.Rows, Name = AdminName };
                string html = CommonHelper.RenderHtml("../html/ScenceList.htm", data);
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