using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// UserEdit 的摘要说明
    /// </summary>
    public class UserEdit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string username = (string)context.Session["LoginUserName"];
            string str;
            if (username == null)
            {
                context.Response.Redirect("UserLogin.ashx?Action=Log");
            }
            else
            {
                string action = context.Request["Action"];
                str = "用户:&nbsp;" + username + "&nbsp;欢迎您";
                if (action == "user_edit")
                {
                    username = (string)context.Session["LoginUserName"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Users where UserName=@Username", new SqlParameter("@Username", username));
                    User user = new User();
                    user = UserDAL.ToUser(dt.Rows[0]);
                    var data = new { Title = "用户信息", Action = "user_update", user, str, Msg = "" };
                    string html = CommonHelper.RenderHtml("../html/UserEdit.htm", data);
                    context.Response.Write(html);
                }
                else if (action == "Adm_edit")
                {
                    username = context.Request["UserName"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Users where UserName=@Username", new SqlParameter("@Username", username));
                    User user = new User();
                    user = UserDAL.ToUser(dt.Rows[0]);
                    var data = new { Title = "用户信息", Action = "Adm_update", user, str };
                    string html = CommonHelper.RenderHtml("../html/UserEdit.htm", data);
                    context.Response.Write(html);

                }
                else if (action == "Adm_update")
                {
                    username = context.Request["userName"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Users where UserName=@username", new SqlParameter("@username", username));
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
                        string realName = context.Request["RealName"];
                        string Email = context.Request["email"];
                        string Gender = context.Request["sex"];
                        UserDAL.Update(username, realName, Gender, Email);
                        context.Response.Redirect("UserList.ashx");
                    }
                }
                else if (action == "user_update")
                {
                    username = context.Request["userName"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Users where UserName=@username", new SqlParameter("@username", username));
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
                        string sex = context.Request["sex"];
                        UserDAL.Update(username, RealName, sex, email);

                        username = (string)context.Session["LoginUserName"];
                        DataTable dt1 = SqlHelper.ExecuteDataTable("select * from T_Users where UserName=@Username", new SqlParameter("@Username", username));
                        User user = new User();
                        user = UserDAL.ToUser(dt1.Rows[0]);
                        var data = new { Title = "用户信息", Action = "user_update", user, str, Msg = "update" };
                        string html = CommonHelper.RenderHtml("../html/UserEdit.htm", data);
                        context.Response.Write(html);
                    }
                }
                else if (action == "User_pwd")
                {
                    string password = context.Request["userNewpwd"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Users where UserName=@username", new SqlParameter("@username", username));
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
                        UserDAL.Update_Pwd(username, password);

                        context.Session.Remove("LoginUserName");
                        context.Response.Redirect("UserLogin.ashx?Action=Log");
                    }
                }
                else if (action == "Delete")
                {
                    username = context.Request["UserName"];
                    SqlHelper.ExecuteNonQuery("Delete from T_Users where UserName=@UserName", new SqlParameter("@UserName", username));
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