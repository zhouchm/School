using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Schoolxm
{
    public class ScenceDAL
    {
        //public void DeleteById(Guid Id)
        //{
        //    //软删除
        //    SqlHelper.ExecuteNonQuery("Update T_Users Set IsDeleted=1 where Id=@Id",
        //        new SqlParameter("@Id", Id));
        //}

        public Scence[] ListAll()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Scence");
            Scence[] scence = new Scence[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                scence[i] = ToScence(dt.Rows[i]);
            }
            return scence;
        }
        public static ConNode[] ConNodeListAll()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_ConNode");
            ConNode[] ConNode = new ConNode[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ConNode[i] = ToConNode(dt.Rows[i]);
            }
            return ConNode;
        }
        public static void Insert(Scence scence)
        {
            //bit类型，在sql语句中要写0、1
            //在.net中要用bool表示
            string time = DateTime.Now.ToLocalTime().ToString();
            SqlHelper.ExecuteNonQuery(@"insert into T_Scence(
                SceId,ScenceName,utime) values(@SceId,@ScenceName,@utime)",
                    new SqlParameter("@SceId", scence.SceId),
                    new SqlParameter("@ScenceName", scence.ScenceName),
                    new SqlParameter("@utime",time));
        }

        public static ConNode ToConNode(DataRow row)
        {
            ConNode ConNode = new ConNode();
            ConNode.Node_sn = (string)row["Node_sn"];
            ConNode.Node_name = (string)row["Node_name"];
            return ConNode;
        }

        public static Scence ToScence(DataRow row)
        {
            Scence scence = new Scence();
            scence.SceId = (string)row["SceId"];
            scence.ScenceName = (string)row["ScenceName"];
            return scence;
        }

        /// <summary>
        /// 不更新密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        public static void Update(string ScenceName, string SceId)
        {
            string time = DateTime.Now.ToLocalTime().ToString();
            SqlHelper.ExecuteNonQuery("update T_Scence set ScenceName=@ScenceName,utime=@utime where SceId=@SceId",
                new SqlParameter("@ScenceName", ScenceName),
                new SqlParameter("@utime",time),
                new SqlParameter("@SceId", SceId));
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        /// <param name="pwd"></param>
        public static void Update_Pwd(string userName, string pwd)
        {
            SqlHelper.ExecuteNonQuery("update T_Admin set Password=@Password where UserName=@UserName",
               new SqlParameter("@Password", pwd),
               new SqlParameter("@UserName", userName));
        }

        //public Admin GetByuserName(string userName)
        //{
        //    DataTable table = SqlHelper.ExecuteDataTable("select * from T_Admin where UserName=@UserName ",
        //        new SqlParameter("@UserName", userName));
        //    if (table.Rows.Count <= 0)
        //    {
        //        return null;
        //    }
        //    else if (table.Rows.Count > 1)
        //    {
        //        throw new Exception("存在重名用户！");
        //    }
        //    else
        //    {
        //        return ToAdmin(table.Rows[0]);
        //    }
        //}

        /*  public User GetByUserName(string userName)
          {
              DataTable table = SqlHelper.ExecuteDataTable("select * from T_Operator where UserName=@UserName and IsDeleted=0",
                  new SqlParameter("@UserName", userName));
              if (table.Rows.Count <= 0)
              {
                  return null;
              }
              else if (table.Rows.Count > 1)
              {
                  throw new Exception("存在重名用户！");
              }
              else
              {
                  return ToOperator(table.Rows[0]);
              }
          }*/
    }
}