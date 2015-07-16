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
    public class AdminEdit : IHttpHandler, IRequiresSessionState
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
                string username = AdminName;
                string action = context.Request["Action"];
                if (action == "Adm_edit")
                {
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Admin where UserName=@UserName", new SqlParameter("@UserName", username));
                    Admin admin = new Admin();
                    admin = AdminDAL.ToAdmin(dt.Rows[0]);
                    var data = new { Title = "现代科技体验中心", Action = "Adm_update", admin, Name = username };
                    string html = CommonHelper.RenderHtml("../html/AdminEdit.htm", data);
                    context.Response.Write(html);
                }
                else if (action == "Adm_update")
                {
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Admin where UserName=@UserName", new SqlParameter("@UserName", username));
                    if (dt.Rows.Count <= 0)
                    {
                        context.Response.Write("找不到用户名" + username + "用户");
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        context.Response.Write("错误！出现重名用户！");
                    }
                    else
                    {
                        string RealName = context.Request["RealName"];
                        string email = context.Request["email"];
                        string address = context.Request["address"];
                        AdminDAL.Update(username, address, email, RealName);

                        context.Response.Redirect("AdminEdit.ashx?Action=Adm_edit");
                    }
                }
                else if (action == "Admin_pwd")
                {
                    string password = context.Request["NewPassword"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Admin where UserName=@UserName", new SqlParameter("@UserName", username));
                    if (dt.Rows.Count <= 0)
                    {
                        context.Response.Write("找不到用户名" + username + "用户");
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        context.Response.Write("错误！出现重名用户！");
                    }
                    else
                    {
                        AdminDAL.Update_Pwd(username, password);
                        context.Session.Remove("LoginAdminName");
                        var data = new { Title = "现代科技体验中心", Msg = "密码修改成功，请重新登录！" };
                        string html = CommonHelper.RenderHtml("../html/AdminLogin.htm", data);
                        context.Response.Write(html);
                    }
                }
                else if (action == "Delete")
                {
                    SqlHelper.ExecuteNonQuery("Delete from T_Admin where UserName=@UserName", new SqlParameter("@UserName", username));
                    context.Response.Redirect("AdminList.ashx");
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