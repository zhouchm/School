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
    /// UserEditAtAdmin 的摘要说明
    /// </summary>
    public class UserEditAtAdmin : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string AdminName = (string)context.Session["LoginAdminName"];
            if (AdminName == null)
            {
                var data = new { Title = "现代科技体验中心" };
                string html = CommonHelper.RenderHtml("../html/AdminLogin.htm", data);
                context.Response.Write(html);
            }
            else
            {
                string action = context.Request["Action"];
                if (action == "Adm_edit")
                {
                    string username = context.Request["UserName"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Users where UserName=@Username", new SqlParameter("@Username", username));
                    User user = new User();
                    user = UserDAL.ToUser(dt.Rows[0]);
                    var data = new { Title = "现代科技体验中心", Action = "Adm_update", Name = AdminName, user };
                    string html = CommonHelper.RenderHtml("../html/UserEditAtAdmin.htm", data);
                    context.Response.Write(html);
                }
                else if (action == "Adm_update")
                {
                    string UserName = context.Request["UserName"];
                    string Email = context.Request["Email"];
                    string Gender = context.Request["Gender"];
                    string RealName = context.Request["RealName"];
                    UserDAL.Update(UserName, RealName, Gender, Email);
                    context.Response.Redirect("UserList.ashx");
                }
                else if (action == "Delete")
                {
                    string username = context.Request["UserName"];
                    SqlHelper.ExecuteNonQuery("Delete from T_Users where UserName=@UserName", new SqlParameter("@UserName", username));
                    context.Response.Redirect("UserList.ashx");
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