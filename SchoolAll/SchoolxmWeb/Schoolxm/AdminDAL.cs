using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Schoolxm
{
    public class AdminDAL
    {
        //public void DeleteById(Guid Id)
        //{
        //    //软删除
        //    SqlHelper.ExecuteNonQuery("Update T_Users Set IsDeleted=1 where Id=@Id",
        //        new SqlParameter("@Id", Id));
        //}

        public Admin[] ListAll()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Admin");
            Admin[] admin = new Admin[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                admin[i] = ToAdmin(dt.Rows[i]);
            }
            return admin;
        }

        public static void Insert(Admin admin)
        {
            //bit类型，在sql语句中要写0、1
            //在.net中要用bool表示
            SqlHelper.ExecuteNonQuery(@"insert into T_Admin(
                UserName,Password,address,email,RealName) values(@UserName,@Password,@address,@email,@RealName)",
                    new SqlParameter("@UserName", admin.UserName),
                    new SqlParameter("@Password", admin.Password),
                    new SqlParameter("@address", admin.address),
                    new SqlParameter("@email", admin.email),
                    new SqlParameter("@RealName", admin.RealName));
        }

        public static Admin ToAdmin(DataRow row)
        {
            Admin admin = new Admin();
            admin.UserName = (string)row["UserName"];
            admin.Password = (string)row["Password"];
            admin.address = (string)row["address"];
            admin.email = (string)row["email"];
            admin.RealName = (string)row["RealName"];
            return admin;
        }

        /// <summary>
        /// 不更新密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        public static void Update(string userName, string address, string email, string realName)
        {
            SqlHelper.ExecuteNonQuery("update T_Admin set address=@address,email=@email,RealName=@RealName where UserName=@UserName",
                new SqlParameter("@address", address),
                new SqlParameter("@email", email),
                new SqlParameter("@RealName", realName),
                new SqlParameter("@UserName", userName));
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

        public Admin GetByuserName(string userName)
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from T_Admin where UserName=@UserName ",
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
                return ToAdmin(table.Rows[0]);
            }
        }

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