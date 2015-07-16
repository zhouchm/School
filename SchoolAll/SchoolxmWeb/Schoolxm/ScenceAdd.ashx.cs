using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Text;
using System.Web.SessionState;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// AdminAdd 的摘要说明
    /// </summary>
    public class ScenceAdd : IHttpHandler, IRequiresSessionState
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
                string action = context.Request["Action"];
                if (action == "add")
                {
                    Scence Scence = new Scence();
                    string s = context.Request["SceId"];
                    s = Convert.ToInt32(s) + "";
                    int num = (int)SqlHelper.ExecuteScalar("select COUNT(*) from T_Scence where SceId=@SceId", new SqlParameter("@SceId", s));
                    if (num <= 0)
                    {
                        Scence.SceId = s;
                        Scence.ScenceName = context.Request["ScenceName"];  //初始密码为用户名
                        ScenceDAL.Insert(Scence);
                        context.Response.Redirect("ScenceList.ashx");
                    }
                    else
                    {
                        var data = new { Title = "现代科技体验中心", Name = AdminName, Msg = "当前编号已存在！"};
                        string html = CommonHelper.RenderHtml("../html/ScenceAdd.htm", data);
                        context.Response.Write(html);
                    }
                }
                else
                {
                    var data = new { Title = "现代科技体验中心", Name = AdminName, Msg = "" };
                    string html = CommonHelper.RenderHtml("../html/ScenceAdd.htm", data);
                    context.Response.Write(html);
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