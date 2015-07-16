using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Schoolxm
{
    /// <summary>
    /// Example 的摘要说明
    /// </summary>
    public class Example : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            DataTable dt = SqlHelper.ExecuteDataTable("select top 4 * from T_News order by time desc;");
            var data = new { Title = "主页", News = dt.Rows };
            string html = CommonHelper.RenderHtml("../html/Example.htm", data);
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