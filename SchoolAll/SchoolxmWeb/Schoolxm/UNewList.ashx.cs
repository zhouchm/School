using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;

namespace Schoolxm
{
    /// <summary>
    /// UNewList 的摘要说明
    /// </summary>
    public class UNewList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string name = (string)context.Session["LoginUserName"];
            if (name == null)
            {
                DataTable ne = SqlHelper.ExecuteDataTable("select * from T_News");
                string str = @"<a href=""/UserLogin.ashx?Action=Log"">登入</a>&nbsp;|&nbsp;<a href=""/UserRegister.ashx?UserReg=Reg"">注册</a>";
                var data = new { Title = "新闻列表", ne = ne.Rows, str };
                string html = CommonHelper.RenderHtml("../html/UewList.htm", data);
                context.Response.Write(html);
            }
            else
            {
                DataTable ne = SqlHelper.ExecuteDataTable("select * from T_News");
                string str = "用户:&nbsp;" + name + "&nbsp;欢迎您";
                var data = new { Title = "新闻列表", ne = ne.Rows, str };
                string html = CommonHelper.RenderHtml("../html/UewList.htm", data);
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