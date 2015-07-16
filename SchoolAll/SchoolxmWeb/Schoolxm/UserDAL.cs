using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Schoolxm
{
    public class UserDAL
    {
        public void DeleteById(Guid Id)
        {
            //软删除
            SqlHelper.ExecuteNonQuery("Update T_Users Set IsDeleted=1 where Id=@Id",
                new SqlParameter("@Id", Id));
        }

        public User[] ListAll()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Users");
            User[] users = new User[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                users[i] = ToUser(dt.Rows[i]);
            }
            return users;
        }

        public static void Insert(User user)
        {
            //bit类型，在sql语句中要写0、1
            //在.net中要用bool表示
            string time = DateTime.Now.ToLocalTime().ToString();
            SqlHelper.ExecuteNonQuery(@"insert into T_Users(
                UserName,Password,RealName,Gender,Email,updatetime) values(@UserName,@Password,@RealName,@Gender,@Email,@updatetime)",
                    new SqlParameter("@UserName", user.UserName),
                    new SqlParameter("@Password", user.Password),
                    new SqlParameter("@RealName", user.RealName),
new SqlParameter("@Gender", user.Gender),
                    new SqlParameter("@Email", user.Email),
                    new SqlParameter("@updatetime",time));
        }

        public static User ToUser(DataRow row)
        {
            User user = new User();
            user.UserName = (string)row["UserName"];
            user.Password = (string)row["Password"];
            user.RealName = (string)row["RealName"];
            user.Gender = (string)row["Gender"];
            user.Email = (string)row["Email"];
            return user;
        }

        /// <summary>
        /// 不更新密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        public static void Update(string userName, string realName, string Gender, string email)
        {
            string time = DateTime.Now.ToLocalTime().ToString();
            SqlHelper.ExecuteNonQuery("update T_Users set RealName=@realName,Gender=@Gender,Email=@Email,updatetime=@updatetime where UserName=@userName",
                new SqlParameter("@realName", realName),
                new SqlParameter("@Gender", Gender),
                new SqlParameter("@Email", email),
                new SqlParameter("@updatetime",time),
                new SqlParameter("@userName", userName));
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        /// <param name="pwd"></param>
        public static void Update_Pwd(string userName, string pwd)
        {
            string time = DateTime.Now.ToLocalTime().ToString();
            SqlHelper.ExecuteNonQuery("update T_Users set Password=@Password, updatetime=@updatetime where UserName=@userName",
               new SqlParameter("@Password", pwd),
               new SqlParameter("@updatetime", time),
               new SqlParameter("@userName", userName));
        }

        public User GetByuserName(string userName)
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from T_User where UserName=@userName ",
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
                return ToUser(table.Rows[0]);
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