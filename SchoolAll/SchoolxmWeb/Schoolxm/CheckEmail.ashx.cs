using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// CheckEmail 的摘要说明
    /// </summary>
    public class CheckEmail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string email = context.Request["Email"];
            int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Users where Email=@email", new SqlParameter("@email", email));
            if (count <= 0)
            {
                context.Response.Write("ok");//用户名不重复
                context.Response.End();
            }
            else
            {
                context.Response.Write("error");//用户名重复
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