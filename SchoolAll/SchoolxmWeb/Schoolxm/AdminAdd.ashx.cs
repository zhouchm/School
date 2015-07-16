using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Text;

namespace Schoolxm
{
    /// <summary>
    /// AdminAdd 的摘要说明
    /// </summary>
    public class AdminAdd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            if (context.Session != null)//初始为==
            {
                context.Response.Redirect("AdminLogin.ashx");
            }
            else
            {
                string action = context.Request["Action"];
                if (action == "add")
                {
                    Admin admin = new Admin();
                    admin.UserName = context.Request["UserName"];
                    admin.Password = context.Request["UserName"];  //初始密码为用户名
                    admin.address = context.Request["address"];
                    admin.email = context.Request["email"];
                    admin.RealName = context.Request["RealName"];
                    AdminDAL.Insert(admin);
                    context.Response.Redirect("AdminList.ashx");
                }
                else
                {
                    string html = CommonHelper.RenderHtml("../html/AdminAdd.htm", null);
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