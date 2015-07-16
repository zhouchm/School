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
    /// SelectClass 的摘要说明
    /// </summary>
    public class SelectClass : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["Action"];
            string AdminName = (string)context.Session["LoginAdminName"];
            if (AdminName == null)
            {
                var data = new { Title = "现代科技体验中心", Msg = "" };
                string html = CommonHelper.RenderHtml("../html/AdminLogin.htm", data);
                context.Response.Write(html);
            }
            else
            {
                if (action == "NoClass")
                {
                    string Count = context.Request["count"];//已预约人数
                    string TimeofDay = context.Request["TimeofDay"];//当前时间
                    string TimeofAP = context.Request["TimeofAP"];
                    DataTable grade = SqlHelper.ExecuteDataTable("select distinct Grade from T_Student order by Grade desc");
                   // DataTable grade = SqlHelper.ExecuteDataTable("select distinct Grade from T_Student");
                    //context.Response.Write("已预约人数" + Count);//测试Count是否传值正确
                    //context.Response.Write("时间安排" + TimeofDay + TimeofAP);//测试时间传值
                    var data = new
                    {
                        Title = "选择班级之前",
                        Name = AdminName,
                        day = TimeofDay,
                        time = TimeofAP,
                        count = Count,//已预约人数
                        Grade=grade.Rows
                    };
                    string html = CommonHelper.RenderHtml("../html/SelectClass.htm", data);
                    context.Response.Write(html);
                }
                else if (action == "HaveClass")
                {
                    string Count = context.Request["Count"];//已预约人数
                    string TimeofDay = context.Request["Day"];//当前时间
                    string TimeofAP = context.Request["Time"];
                    //context.Response.Write("已预约人数" + Count);//测试Count是否传值正确
                    //context.Response.Write("时间安排" + TimeofDay + TimeofAP);//测试时间传值
                    //string str = @"用户:&nbsp;" + username + "&nbsp;欢迎您";
                    string Grade = context.Request["grade"];
                    string Class = context.Request["class"];
                    DateTime now = DateTime.Now;
                    int month = now.Month;
                    int year=now.Year;
                    if (month < 9) year = year - 1;
                    
                    DataTable student = SqlHelper.ExecuteDataTable("select * from T_Student where Grade=@Grade and Class=@class and Year=@year", new SqlParameter("@Grade", Grade),
                        new SqlParameter("@Class", Class),new SqlParameter("@year", year.ToString()));

                    DataTable grade = SqlHelper.ExecuteDataTable("select distinct Grade from T_Student order by Grade desc");

                    Order[] order = new Order[student.Rows.Count];
                    for (int i = 0; i < student.Rows.Count; ++i)
                    {
                        order[i] = new Order();
                        order[i].Id = i + 1;
                        order[i].StuAccount = (string)student.Rows[i]["StuAccount"];
                        order[i].StuName = (string)student.Rows[i]["StuName"];

                    }
                    var data = new
                    {
                        Title = "用户列表",
                        //stu = student.Rows,
                        groupOrder = order,
                        Name = AdminName,
                        count = Count,//已预约人数
                        Grade=grade.Rows,
                        CurrentGrade = Grade,
                        CurrentClass = Class,
                        day = TimeofDay,
                        time = TimeofAP,
                        stuCount = student.Rows.Count//学生人数
                    };
                    string html = CommonHelper.RenderHtml("../html/SelectClass.htm", data);
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