using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Schoolxm
{
    public class TimeSetDAL
    {
        public static TimeSet List(string date, string time)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_TimeSet where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP",
                new SqlParameter("@TimeOfDay", date), new SqlParameter("@TimeOfAP", time));
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                return ToTimeSet(dt.Rows[0]);
            }
        }

        public static TimeSet[] ListAll()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_TimeSet");
            TimeSet[] ts = new TimeSet[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ts[i] = ToTimeSet(dt.Rows[i]);
            }
            return ts;
        }

        public static void Insert(TimeSet ts)
        {
            SqlHelper.ExecuteNonQuery(@"insert into T_TimeSet(TimeOfDay,TimeOfAP,TimeOfSet) values(@TimeOfDay,@TimeOfAP,@TimeOfSet)",
                new SqlParameter("@TimeOfDay", ts.TimeOfDay),
                new SqlParameter("@TimeOfAP", ts.TimeOfAP),
                new SqlParameter("@TimeOfSet", ts.TimeOfSet));
        }

        public static void Delete(string date, string time)
        {
            SqlHelper.ExecuteNonQuery(@"delete from T_TimeSet where TimeOfDay=@TimeOfDay and TimeOfAP=@TimeOfAP",
                new SqlParameter("@TimeOfDay", date), new SqlParameter("@TimeOfAP", time));
        }

        public static TimeSet ToTimeSet(DataRow row)
        {
            TimeSet ts = new TimeSet();
            ts.TimeOfDay = (string)row["TimeOfDay"];
            ts.TimeOfAP = (string)row["TimeOfAP"];
            ts.TimeOfSet = (string)row["TimeOfSet"];
            return ts;
        }
    }
}