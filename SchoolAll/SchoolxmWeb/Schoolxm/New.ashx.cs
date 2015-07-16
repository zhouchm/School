using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// New 的摘要说明
    /// </summary>
    public class New : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string AdminName = (string)context.Session["LoginAdminName"];
            string time = context.Request["time"];
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_News where time=@time", new SqlParameter("@time", time));
            string name = dt.Rows[0]["name"].ToString();
            string title = dt.Rows[0]["title"].ToString();
            string con = dt.Rows[0]["news"].ToString();
            var data = new { Title = "新闻", name, time, title, con, Name = AdminName };
            string html = CommonHelper.RenderHtml("../html/New.htm", data);
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