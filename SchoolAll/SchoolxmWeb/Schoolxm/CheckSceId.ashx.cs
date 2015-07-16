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
    public class CheckSceId : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string SceId = context.Request["SceId"];
            int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Scence where SceId=@SceId", new SqlParameter("@SceId", SceId));
            if (count <= 0)
            {
                context.Response.Write("ok");//场景名不重复
                context.Response.End();
            }
            else
            {
                context.Response.Write("error");//场景名重复
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