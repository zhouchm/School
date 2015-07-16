using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// SceAddDev 的摘要说明
    /// </summary>
    public class SceAddDev : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string SceId=context.Request["SceId"];
            string Node_sn=context.Request["Node_sn"];
            string Node_name=context.Request["Node_name"];
            string Node_control=context.Request["Control"];
            SqlHelper.ExecuteNonQuery(@"insert into T_SceDevList(
               SceId,DevId,DevName,DevControl) values(@SceId,@DevId,@DevName,@DevControl)",
                    new SqlParameter("@SceId", SceId),
                    new SqlParameter("@DevId", Node_sn),
                    new SqlParameter("@DevName", Node_name),
                    new SqlParameter("@DevControl", Node_control));
            string url = "ScenceDeviceList.ashx?SceId=" + SceId;
            context.Response.Redirect(url);
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