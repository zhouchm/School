using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.SessionState;

namespace Schoolxm
{
    /// <summary>
    /// OrederNumber 的摘要说明
    /// </summary>
    public class OrederNumber : IHttpHandler, IRequiresSessionState
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
                if (action == "Set")
                {
                    var data = new { Name = AdminName, C = "" };

                    string html = CommonHelper.RenderHtml("../html/OrderNumber.htm", data);
                    context.Response.Write(html);
                }
                else  if (action == "Set_Add")
                {
                    string year = context.Request["year"];
                    string month = context.Request["month"];
                    string day = context.Request["day"];
                    string time = context.Request["time"];
                    string sel = context.Request["groub"];

                    if (Convert.ToInt32(month) < 10)
                        month = "0" + month;
                    if (Convert.ToInt32(day) < 10)
                        day = "0" + day;
                    if (time == "s")
                        time = "上午";
                    else
                        time = "下午";
                    TimeSet ts = new TimeSet();
                    ts.TimeOfDay = year + "年" + month + "月" + day + "日";
                    ts.TimeOfAP = time;
                    try
                    {
                        if (sel == "p")
                        {
                            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_TimeUser where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP", new SqlParameter("@TimeOfDay", ts.TimeOfDay), new SqlParameter("@TimeOfAP", ts.TimeOfAP));
                            int c = dt.Rows.Count;
                            var data = new { Name = AdminName, TS = dt.Rows, C = c };

                            string html = CommonHelper.RenderHtml("../html/OrderNumber.htm", data);
                            context.Response.Write(html);
                        }
                        else if (sel == "g")
                        {
                            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_TimeStudent where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP", new SqlParameter("@TimeOfDay", ts.TimeOfDay), new SqlParameter("@TimeOfAP", ts.TimeOfAP));
                            int c = dt.Rows.Count;
                            var data = new { Name = AdminName, TS = dt.Rows, C = c };

                            string html = CommonHelper.RenderHtml("../html/OrderNumber.htm", data);
                            context.Response.Write(html);
                        }
                    }
                    catch
                    {
                        var data = new { Name = AdminName, TS = "0" };

                        string html = CommonHelper.RenderHtml("../html/OrderNumber.htm", data);
                        context.Response.Write(html);
                    }
                }
                else if (action == "Set_Delete")
                {
                    string date = context.Request["Date"];
                    string time = context.Request["Time"];
                    TimeSetDAL.Delete(date, time);
                    context.Response.Redirect("OrderSet.ashx");
                }
                else if (action == "Set_OpenTime")
                {
                    string opentime = context.Request["opentime"];
                    SqlHelper.ExecuteNonQuery("update T_VisitTime set VisitTime=@opentime where TID='1'", new SqlParameter("@opentime", opentime));
                    context.Response.Redirect("OrderSet.ashx");
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