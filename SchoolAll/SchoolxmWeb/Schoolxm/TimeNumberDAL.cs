using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Schoolxm
{
    public class TimeNumberDAL
    {
        public static TimeNumber List(string date, string time)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_TimeNumber where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP",
                new SqlParameter("@TimeOfDay", date),
                new SqlParameter("@TimeOfAP", time));
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                return ToTimeNumber(dt.Rows[0]);
            }   
        }

        public static void Insert(TimeNumber tn)
        {
            SqlHelper.ExecuteNonQuery(@"insert into T_TimeNumber(TimeOfDay,TimeOfAP,OrderUser) values(@TimeOfDay,@TimeOfAP,@OrderUser)",
                new SqlParameter("@TimeOfDay", tn.TimeOfDay),
                new SqlParameter("@TimeOfAP", tn.TimeOfAP),
                new SqlParameter("@OrderUser", tn.OrderUser));
        }

        public static void Update(string date, string time)
        {
            SqlHelper.ExecuteNonQuery("update T_TimeNumber set OrderUser=OrderUser+1 where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP",
                new SqlParameter("@TimeOfDay", date),
                new SqlParameter("@TimeOfAP", time));
        }

        public static TimeNumber ToTimeNumber(DataRow row)
        {
            TimeNumber tn = new TimeNumber();
            tn.TimeOfDay = (string)row["TimeOfDay"];
            tn.TimeOfAP = (string)row["TimeOfAP"];
            tn.OrderUser = Convert.ToInt32(row["OrderUser"]);
            return tn;
        }
    }
}