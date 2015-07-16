using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// CheckIdNumber 的摘要说明
    /// </summary>
    public class CheckIdNumber : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string IdNumber = context.Request["IdNumber"];
            int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Users where IdNumber=@IdNumber", new SqlParameter("@IdNumber", IdNumber));
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