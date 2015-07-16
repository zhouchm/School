using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Data;

namespace Schoolxm
{
    /// <summary>
    /// AdminAddMessage 的摘要说明
    /// </summary>
    public class AdminAddMessage : IHttpHandler, IRequiresSessionState
    {
        public static String TextToHtml(String sourcestr)
        {
            int strlen;
            String restring = "", destr = "";
            strlen = sourcestr.Length;
            for (int i = 0; i < strlen; i++)
            {
                char ch = sourcestr[i];
                switch (ch)
                {
                    case '<':
                        destr = "<";
                        break;
                    case '>':
                        destr = ">";
                        break;
                    case '\"':
                        destr = "\"";
                        break;
                    case '&':
                        destr = "&";
                        break;
                    case '\n':
                        destr = "<br>";
                        break;
                    case ' ':
                        destr = " ";
                        break;
                    default:
                        destr = "" + ch;
                        break;
                }
                restring = restring + destr;
            }
            return "" + restring;
        }

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
                if (action == "Add")
                {
                    var data = new { Title = "添加新闻", Name = AdminName };
                    string html = CommonHelper.RenderHtml("../html/AdminAddMessage.htm", data);
                    context.Response.Write(html);
                }
                else if (action == "AddMessage")
                {
                    string title = context.Request["NewsName"];
                    string news = context.Request["NewContext"];
                    string time = DateTime.Now.ToString();
                    string name = AdminName;
                    string str = TextToHtml(news);
                    SqlHelper.ExecuteNonQuery(@"insert into T_News(name,title,news,time) values(@name,@title,@news,@time)",
                        new SqlParameter("@name", name),
                        new SqlParameter("@title", title),
                        new SqlParameter("@news", str),
                        new SqlParameter("@time", time));
                    context.Response.Redirect("NewList.ashx");
                }
                else if (action == "New_Edit")
                {
                    string time = context.Request["time"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_News where time=@time", new SqlParameter("@time", time));
                    string title = dt.Rows[0]["title"].ToString();
                    string con = dt.Rows[0]["news"].ToString();
                    string str = con.Replace("<br>", "\n");
                    var data = new { Title = "编辑新闻", time, title, str, Name = AdminName };
                    string html = CommonHelper.RenderHtml("../html/New_Edit.htm", data);
                    context.Response.Write(html);
                }
                else if (action == "New_Update")
                {
                    string time = context.Request["time"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_News where time=@time", new SqlParameter("@time", time));
                    if (dt.Rows.Count <= 0)
                    {
                        context.Response.Write("找不到此条新闻");
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        context.Response.Write("错误！数据重复！");
                    }
                    else
                    {
                        string title = context.Request["NewsName"];
                        string news = context.Request["NewContext"];
                        string str = TextToHtml(news);
                        SqlHelper.ExecuteNonQuery("update T_News set title=@title,news=@news where time=@time",
                            new SqlParameter("@title", title),
                            new SqlParameter("@news", str),
                            new SqlParameter("@time", time));
                        context.Response.Redirect("NewList.ashx");
                    }
                }
                else if (action == "Delete")
                {
                    string time = context.Request["time"];
                    SqlHelper.ExecuteNonQuery("Delete from T_News where time=@time", new SqlParameter("@time", time));
                    context.Response.Redirect("NewList.ashx");
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