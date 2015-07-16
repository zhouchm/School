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
    /// GroupOrder 的摘要说明
    /// </summary>
    public class GroupOrder : IHttpHandler, IRequiresSessionState
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
                string state = null;
                Order[] order = new Order[14];
                for (int i = 0; i < 14; ++i)
                {
                    order[i] = new Order();
                    if (i % 2 == 0)
                    {
                        order[i].Id = i + 1;
                        order[i].Date = DateTime.Now.AddDays(i / 2).ToString("yyyy年MM月dd日");
                        order[i].Time = "上午";

                        string orderNum = (string)SqlHelper.ExecuteScalar("select OrderUser from T_TimeNumber where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP"
                            , new SqlParameter[] { new SqlParameter("@TimeOfDay", order[i].Date), 
                                new SqlParameter("@TimeOfAP", order[i].Time)});
                        if (Convert.ToInt32(orderNum) <= 0)
                        {
                            order[i].NotOrderNumber = 50;//可以预约的人数
                            order[i].OrderNumber = 0;//已预约人数
                        }
                        else
                        {
                            order[i].OrderNumber = Convert.ToInt32(orderNum);
                            order[i].NotOrderNumber = 50 - Convert.ToInt32(orderNum);
                        }

                        state = order[i].Date;
                    }
                    else
                    {
                        order[i].Id = i + 1;
                        order[i].Date = state;
                        order[i].Time = "下午";

                        string orderNum = (string)SqlHelper.ExecuteScalar("select OrderUser from T_TimeNumber where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP"
                            , new SqlParameter[] { new SqlParameter("@TimeOfDay", order[i].Date), 
                                new SqlParameter("@TimeOfAP", order[i].Time)});
                        if (Convert.ToInt32(orderNum) <= 0)
                        {
                            order[i].NotOrderNumber = 50;//可以预约的人数
                            order[i].OrderNumber = 0;//已预约人数
                        }
                        else
                        {
                            order[i].OrderNumber = Convert.ToInt32(orderNum);
                            order[i].NotOrderNumber = 50 - Convert.ToInt32(orderNum);
                        }
                    }
                }
                var data = new { Order = order, Name = AdminName };
                string html = CommonHelper.RenderHtml("../html/GroupOrder.htm", data);
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