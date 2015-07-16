using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Schoolxm
{
    public class TimeUserDAL
    {
        public static TimeUser List(string date, string time, string name)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_TimeUser where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP and UserName=@UserName",
                new SqlParameter("@TimeOfDay", date),
                new SqlParameter("@TimeOfAP", time),
                new SqlParameter("@UserName", name));
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                return ToTimeUser(dt.Rows[0]);
            }   
        }

        public static void Insert(TimeUser tu)
        {
            SqlHelper.ExecuteNonQuery(@"insert into T_TimeUser(TimeOfDay,TimeOfAP,UserName) values(@TimeOfDay,@TimeOfAP,@UserName)",
                new SqlParameter("@TimeOfDay", tu.TimeOfDay),
                new SqlParameter("@TimeOfAP", tu.TimeOfAP),
                new SqlParameter("@UserName", tu.UserName));
        }

        public static TimeUser ToTimeUser(DataRow row)
        {
            TimeUser tu = new TimeUser();
            tu.TimeOfDay = (string)row["TimeOfDay"];
            tu.TimeOfAP = (string)row["TimeOfAP"];
            tu.UserName = (string)row["UserName"];
            return tu;
        }
    }
}