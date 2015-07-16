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
    /// AdminEdit 的摘要说明
    /// </summary>
    public class ScenceEdit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string SceId = null;
            string AdminName = (string)context.Session["LoginAdminName"];
            if (AdminName == null)
            {
                var data = new { Title = "现代科技体验中心", Msg = "" };
                string html = CommonHelper.RenderHtml("../html/AdminLogin.htm", data);
                context.Response.Write(html);
            }
            else
            {
                string action = context.Request["Action"];
                if (action == "Sce_edit")
                {
                    SceId = context.Request["SceId"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Scence where SceId=@SceId", new SqlParameter("@SceId", SceId));
                    Scence Scence = new Scence();
                    Scence = ScenceDAL.ToScence(dt.Rows[0]);
                    var data = new { Title = "现代科技体验中心", Action = "Sce_update", Scence, Name = AdminName };
                    string html = CommonHelper.RenderHtml("../html/ScenceEdit.htm", data);
                    context.Response.Write(html);
                }
                else if (action == "Sce_update")
                {
                    SceId = context.Request["SceId"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Scence where SceId=@SceId", new SqlParameter("@SceId", SceId));
                    if (dt.Rows.Count <= 0)
                    {
                        context.Response.Write("找不到编号为" + SceId + "场景");
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        context.Response.Write("错误！");
                    }
                    else
                    {
                        string ScenceName = context.Request["ScenceName"];
                        ScenceDAL.Update(ScenceName, SceId);
                        context.Response.Redirect("ScenceList.ashx");
                    }
                }
                else if (action == "DevDelete")
                {
                    SceId = context.Request["SceId"];
                    string DevId=context.Request["DevId"];
                    SqlHelper.ExecuteNonQuery("Delete from T_SceDevList where SceId=@SceId and DevId=@DevId", new SqlParameter("@SceId", SceId),
                        new SqlParameter("@DevId", DevId));

                    DataTable DeviceList = SqlHelper.ExecuteDataTable("select * from T_SceDevList where SceId=@SceId", new SqlParameter("@SceId", SceId));
                    var data = new { Title = "现代科技体验中心", SceId, DevList = DeviceList.Rows, Name = AdminName };
                    string html = CommonHelper.RenderHtml("../html/ScenceDeviceList.htm", data);
                    context.Response.Write(html);
                }
                else if (action == "Delete")
                {
                    SceId = context.Request["SceId"];
                    SqlHelper.ExecuteNonQuery("Delete from T_SceDevList where SceId=@SceId ", new SqlParameter("@SceId", SceId));
                    SqlHelper.ExecuteNonQuery("Delete from T_Scence where SceId=@SceId", new SqlParameter("@SceId", SceId));
                    context.Response.Redirect("ScenceList.ashx");
                }
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