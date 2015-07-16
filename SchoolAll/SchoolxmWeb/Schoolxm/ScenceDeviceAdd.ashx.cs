using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// ScenceDeviceAdd 的摘要说明
    /// </summary>
    public class ScenceDeviceAdd : IHttpHandler, IRequiresSessionState
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
                string SceId = context.Request["SceId"];
                DataTable ConNodes = SqlHelper.ExecuteDataTable("select * from T_ConNode where Node_cmode=2");
                var data = new { Title = "现代科技体验中心", SceId, ConNodes = ConNodes.Rows, Name = AdminName };
                string html = CommonHelper.RenderHtml("../html/ConNodeList.htm", data);
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