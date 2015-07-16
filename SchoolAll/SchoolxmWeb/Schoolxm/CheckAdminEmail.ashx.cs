using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// CheckAdminEmail 的摘要说明
    /// </summary>
    public class CheckAdminEmail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string email = context.Request["Email"];
            int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Admin where email=@email", new SqlParameter("@email", email));
            if (count <= 0)
            {
                context.Response.Write("ok");
                context.Response.End();
            }
            else
            {
                context.Response.Write("error");
                context.Response.End();
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