using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using RFD.SSO.Server.Ado;
using RFD.SSO.Server.Model;

namespace RFD.SSO.Server.Dao
{
    public class MyDefaultSiteDao
    {
        public bool Merge(MyDefaultSite mysite)
        {
            //根据职工号、员工密码、删除标志查询该员工是否存在。
            string sql = @" 
                    update MyDefaultSite set DefaultSite = @DefaultSite 
                    where UserCode = @UserCode 

                    INSERT INTO MyDefaultSite(UserCode,DefaultSite)
                    select @UserCode,@DefaultSite
                     where NOT EXISTS ( SELECT 1
                      FROM MyDefaultSite 
                     WHERE UserCode = @UserCode
                       and DefaultSite = @DefaultSite ) ";

            SqlParameter[] sqlParam = 
            {
               new SqlParameter("@UserCode",SqlDbType.VarChar){Value = mysite.UserCode},
               new SqlParameter("@DefaultSite", SqlDbType.VarChar) { Value = mysite.DefaultSite }
            };

            int r = SqlHelper.ExecuteNonQuery(ConnectString.LmsConStr, CommandType.Text, sql, sqlParam);

            return 1 == r;
        }

        public string GetDefaultSite(string usercode)
        {
            //根据职工号、员工密码、删除标志查询该员工是否存在。
            string sql =
                @" SELECT defaultsite FROM  MyDefaultSite(NOLOCK) WHERE isdelete = 0 and usercode=@UserCode";

            SqlParameter[] sqlParam = 
            {
               new SqlParameter("@UserCode",SqlDbType.VarChar){Value = usercode}
            };

            var o = SqlHelper.ExecuteScalar(ConnectString.LmsConStr, CommandType.Text, sql, sqlParam);

            if (null == o)
            {
                return "";
            }
            return o.ToString();
        }


    }
}
