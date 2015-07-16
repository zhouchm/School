using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// CheckUserPwd 的摘要说明
    /// </summary>
    public class CheckUserPwd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string userName = context.Request["UserName"];
            string pwd=context.Request["PassWord"];
            int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Users where UserName=@UserName and Password=@Password", new SqlParameter("@UserName", userName), new SqlParameter("@Password", pwd));
            if (count >= 1)
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