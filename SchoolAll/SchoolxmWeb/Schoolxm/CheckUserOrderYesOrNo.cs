using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schoolxm
{
    public class CheckUserOrderYesOrNo
    {
        public string Check(string date, string time, string username)
        {
            TimeUser tu = TimeUserDAL.List(date, time, username);
            if (tu == null)//未预约
            {
                TimeNumber tn = TimeNumberDAL.List(date, time);
                if (tn == null)
                {
                    return "yes";
                }
                else
                {
                    if (tn.OrderUser < 50)//预约人数未满
                    {
                        return "yes";
                    }
                    else//预约人数已满
                    {
                        return "mian";
                    }
                }
            }
            else//已经预约
            {
                return "no";
            }
        }
    }
}