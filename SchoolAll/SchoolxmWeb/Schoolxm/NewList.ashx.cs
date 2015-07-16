using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// NewList 的摘要说明
    /// </summary>
    public class NewList : IHttpHandler, IRequiresSessionState
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
                DataTable ne = SqlHelper.ExecuteDataTable("select * from T_News");
                var data = new { Title = "新闻列表", ne = ne.Rows, Name = AdminName };
                string html = CommonHelper.RenderHtml("../html/NewList.htm", data);
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