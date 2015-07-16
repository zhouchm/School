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
    /// AdminList 的摘要说明
    /// </summary>
    public class ScenceDeviceList : IHttpHandler, IRequiresSessionState
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
                string SceId=context.Request["SceId"];
                DataTable DeviceList = SqlHelper.ExecuteDataTable("select * from T_SceDevList where SceId=@SceId",new SqlParameter("@SceId",SceId));
                var data = new { Title = "现代科技体验中心", SceId, DevList = DeviceList.Rows, Name = AdminName };
                string html = CommonHelper.RenderHtml("../html/ScenceDeviceList.htm", data);
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