using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using RFD.SSO.Server.Ado;
using RFD.SSO.Server.Domain;
using RFD.SSO.Server.Model;
using RFD.SSO.Server.Util;

namespace RFD.SSO.Server.Dao
{
    public class OnlineUserDao : IDB
    {
        public bool Add(SsoToken ssoToken)
        {
            string sql = @" MERGE INTO OnlineUser c
                                USING 
                                    ( SELECT    @EmployeeCode EmployeeCode ,
                                                @Token Token ,
                                                @LoginIP LoginIP ,
                                                @LoginTime LoginTime ,
                                                @ValidateTime ValidateTime
                                    ) f
                                ON ( c.Token = f.Token )
                                WHEN MATCHED THEN
	                                 UPDATE  SET
				                            EmployeeCode = f.EmployeeCode ,
				                            LoginIP = f.LoginIP ,
                                            LoginTime = f.LoginTime,
				                            ValidateTime = f.ValidateTime
                                WHEN NOT MATCHED THEN
		                            INSERT  (
                                              OnlineId,
				                              EmployeeCode ,
				                              Token ,
				                              LoginIP ,
				                              LoginTime ,
				                              ValidateTime
				                            ) VALUES
				                            ( 
                                              (SELECT ISNULL(MAX(OnlineId),0)+ 1 FROM OnlineUser(NOLOCK)),
                                              f.EmployeeCode ,
				                              f.Token ,
				                              f.LoginIP ,
				                              f.LoginTime ,
				                              f.ValidateTime) ;";

            SqlParameter[] sqlParam = 
            {
               new SqlParameter("@EmployeeCode",SqlDbType.NVarChar,20){Value = ssoToken.UserData.EmployeeCode},
               new SqlParameter("@Token", SqlDbType.VarChar,100) { Value = ssoToken.Token },
               new SqlParameter("@LoginIP", SqlDbType.VarChar,15) { Value =  ssoToken.LoginRequest.IP },
               new SqlParameter("@LoginTime", SqlDbType.DateTime) { Value = ssoToken.LoginTime },
               new SqlParameter("@ValidateTime", SqlDbType.DateTime) { Value = ssoToken.ValidateTime }
            };

            int r = SqlHelper.ExecuteNonQuery(ConnectString.LmsConStr, CommandType.Text, sql, sqlParam);

            return r > 0;
        }

        public bool Update(SsoToken ssoToken)
        {
            return Add(ssoToken);
        }

        public bool Remove(string token)
        {
            string sql = @"DELETE FROM OnlineUser WHERE Token = @Token ";

            var tok = new SqlParameter("@Token", SqlDbType.VarChar, 100) { Value = token };
            int r = SqlHelper.ExecuteNonQuery(ConnectString.LmsConStr, CommandType.Text, sql, tok);

            return r > 0;
        }

        public Dictionary<string, SsoToken> GetAll()
        {
            throw new NotImplementedException();
        }

        public SsoToken GetOne(string token)
        {
            string sql = @" SELECT  em.EmployeeID ,
                                    em.EmployeeCode ,
                                    em.EmployeeName ,
                                    em.StationID ,
                                    ec.Companyname ,
                                    em.DistributionCode ,
                                    em.SysManager ,
                                    ou.Token ,
                                    ou.LoginIP ,
                                    ou.LoginTime ,
                                    ou.ValidateTime
                            FROM    dbo.OnlineUser (NOLOCK) ou
                                    JOIN Employee (NOLOCK) em ON ou.EmployeeCode = em.EmployeeCode
                                    JOIN ExpressCompany (NOLOCK) ec ON em.StationID = ec.expressCompanyid
                            WHERE   em.IsDeleted = 0
                                    AND Token = @Token
                            ORDER BY ValidateTime DESC";

            SqlParameter[] sqlParam = 
            {
               new SqlParameter("@Token",SqlDbType.VarChar){Value = token}
            };

            var ds = SqlHelper.ExecuteDataset(ConnectString.LmsConStr, CommandType.Text, sql, sqlParam);

            if (null == ds || 1 != ds.Tables.Count || 1 != ds.Tables[0].Rows.Count)
            {
                return null;
            }

            var r = ds.Tables[0].Rows[0];
            SsoToken tok = new SsoToken()
            {
                LoginRequest = new LoginRequest()
                {
                    IP = r["LoginIP"].ToString(),
                    LoginId = r["EmployeeCode"].ToString()
                },
                LoginTime = DateTime.Parse(r["LoginTime"].ToString()),
                ValidateTime = DateTime.Parse(r["ValidateTime"].ToString()),
                Token = r["Token"].ToString(),
                UserData = new SsoResponse
                {
                    EmployeeID = r["EmployeeID"].ToString().TryGetInt(),
                    EmployeeCode = r["EmployeeCode"].ToString(),
                    EmployeeName = r["EmployeeName"].ToString(),
                    StationID = r["StationID"].ToString().TryGetInt(),
                    Companyname = r["Companyname"].ToString(),
                    DistributionCode = r["DistributionCode"].ToString(),
                    SysManager = r["SysManager"].ToString().TryGetInt()
                }
            };

            return tok;
        }
    }
}
