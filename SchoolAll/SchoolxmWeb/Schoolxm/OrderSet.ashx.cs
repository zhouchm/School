using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Data;

namespace Schoolxm
{
    /// <summary>
    /// OrderSet 的摘要说明
    /// </summary>
    public class OrderSet : IHttpHandler, IRequiresSessionState
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
                if (action == "Set_Add")
                {
                    string year = context.Request["year"];
                    string month = context.Request["month"];
                    string day = context.Request["day"];
                    string time = context.Request["time"];
                    string incident = context.Request["incident"];

                    if (Convert.ToInt32(month) < 10)
                        month = "0" + month;
                    if (Convert.ToInt32(day) < 10)
                        day = "0" + day;
                    if (time == "s")
                        time = "上午";
                    else
                        time = "下午";
                    if (incident == "b")
                        incident = "闭馆";
                    else
                        incident = "团体参观";

                    TimeSet ts = new TimeSet();
                    ts.TimeOfDay = year + "年" + month + "月" + day + "日";
                    ts.TimeOfAP = time;
                    ts.TimeOfSet = incident;

                    TimeSet timeset = TimeSetDAL.List(ts.TimeOfDay, ts.TimeOfAP);
                    if (timeset == null)
                    {
                        TimeSetDAL.Insert(ts);
                        context.Response.Redirect("OrderSet.ashx");
                    }
                    else
                    {
                        DataTable time2 = SqlHelper.ExecuteDataTable("select * from T_VisitTime where TID='1'");
                        TimeSet[] ts2 = TimeSetDAL.ListAll();
                        var data = new { Name = AdminName, TS = ts2, Time = time2.Rows[0], Msg = "error" };
                        string html = CommonHelper.RenderHtml("../html/OrderSet.htm", data);
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
                else
                {
                    DataTable time = SqlHelper.ExecuteDataTable("select * from T_VisitTime where TID='1'");
                    TimeSet[] ts = TimeSetDAL.ListAll();
                    var data = new { Name = AdminName, TS = ts, Time = time.Rows[0], Msg = "" };
                    string html = CommonHelper.RenderHtml("../html/OrderSet.htm", data);
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