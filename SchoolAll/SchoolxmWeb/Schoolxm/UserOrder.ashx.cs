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
    /// UserOrder 的摘要说明
    /// </summary>
    public class UserOrder : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string username = (string)context.Session["LoginUserName"];
            if (username == null)
            {
                context.Response.Redirect("UserLogin.ashx?Action=Log");
            }
            else
            {
                string action = context.Request["Action"];
                if (action == "Order")
                {
                    string date = context.Request["Date"];
                    string time = context.Request["Time"];
                    TimeNumber tn = TimeNumberDAL.List(date, time);
                    if (tn == null)
                    {
                        TimeUser tu = new TimeUser();
                        tu.TimeOfDay = date;
                        tu.TimeOfAP = time;
                        tu.UserName = username;
                        TimeUserDAL.Insert(tu);

                        TimeNumber t = new TimeNumber();
                        t.TimeOfDay = date;
                        t.TimeOfAP = time;
                        t.OrderUser = 1;
                        TimeNumberDAL.Insert(t);
                    }
                    else
                    {
                        TimeUser tu = new TimeUser();
                        tu.TimeOfDay = date;
                        tu.TimeOfAP = time;
                        tu.UserName = username;
                        TimeUserDAL.Insert(tu);

                        TimeNumberDAL.Update(date, time);
                    }
                    context.Response.Redirect("UserOrder.ashx");
                }
                else
                {
                    string past = null;
                    Order[] order = new Order[14];
                    for (int i = 0; i < 14; ++i)
                    {
                        order[i] = new Order();
                        if (i % 2 == 0)
                        {
                            order[i].Id = i + 1;
                            order[i].Date = DateTime.Now.AddDays(i / 2).ToString("yyyy年MM月dd日");
                            order[i].Time = "上午";

                            TimeNumber tn = TimeNumberDAL.List(order[i].Date, order[i].Time);
                            if (tn == null)
                            {
                                order[i].NotOrderNumber = 0;
                                order[i].OrderNumber = 50;
                            }
                            else
                            {
                                order[i].NotOrderNumber = tn.OrderUser;
                                order[i].OrderNumber = 50 - order[i].NotOrderNumber;
                            }
                            TimeSet timeset = TimeSetDAL.List(order[i].Date, order[i].Time);
                            if (timeset == null)//开馆
                            {
                                CheckUserOrderYesOrNo check = new CheckUserOrderYesOrNo();
                                string s = check.Check(order[i].Date, order[i].Time, username);
                                if (s == "yes")
                                {
                                    order[i].State = "";
                                    order[i].OrderYesOrNo = "White";
                                    order[i].ButtonValue = "可预约";
                                }
                                else if (s == "mian")
                                {
                                    order[i].State = "disabled";
                                    order[i].OrderYesOrNo = "#ddd";
                                    order[i].ButtonValue = "不可预约";
                                }
                                else
                                {
                                    order[i].State = "disabled";
                                    order[i].OrderYesOrNo = "Orange";
                                    order[i].ButtonValue = "已预约";
                                }
                            }
                            else//闭馆
                            {
                                order[i].State = "disabled";
                                order[i].OrderYesOrNo = "#ddd";
                                order[i].ButtonValue = "闭馆";
                            }
                            past = order[i].Date;
                        }
                        else
                        {
                            order[i].Id = i + 1;
                            order[i].Date = past;
                            order[i].Time = "下午";

                            TimeNumber tn = TimeNumberDAL.List(order[i].Date, order[i].Time);
                            if (tn == null)
                            {
                                order[i].NotOrderNumber = 0;
                                order[i].OrderNumber = 50;
                            }
                            else
                            {
                                order[i].NotOrderNumber = tn.OrderUser;
                                order[i].OrderNumber = 50 - order[i].NotOrderNumber;
                            }
                            TimeSet timeset = TimeSetDAL.List(order[i].Date, order[i].Time);
                            if (timeset == null)//开馆
                            {
                                CheckUserOrderYesOrNo check = new CheckUserOrderYesOrNo();
                                string s = check.Check(order[i].Date, order[i].Time, username);
                                if (s == "yes")
                                {
                                    order[i].State = "";
                                    order[i].OrderYesOrNo = "White";
                                    order[i].ButtonValue = "可预约";
                                }
                                else
                                {
                                    order[i].State = "disabled";
                                    order[i].OrderYesOrNo = "Orange";
                                    order[i].ButtonValue = "已预约";
                                }
                            }
                            else//闭馆
                            {
                                order[i].State = "disabled";
                                order[i].OrderYesOrNo = "#ddd";
                                order[i].ButtonValue = "闭馆";
                            }
                        }
                    }
                    string str = @"用户:&nbsp;" + username + "&nbsp;欢迎您";
                    var data = new { Title = "个人预约", str, Order = order };
                    string html = CommonHelper.RenderHtml("../html/UserOrder.htm", data);
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