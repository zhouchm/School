using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// CheckAdminUserName 的摘要说明
    /// </summary>
    public class CheckAdminUserName : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string userName = context.Request["UserName"];
            if (userName.Contains("毛泽东") || userName.Contains("管理员"))
            {
                context.Response.Write("forbid");
                return;
            }

            int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Admin where UserName=@UserName", new SqlParameter("@UserName", userName));
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