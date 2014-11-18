using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using LMS.Util;
using LMS.Util.Security;
using Microsoft.ApplicationBlocks.Data;
using RFD.SSO.Server.Ado;
using RFD.SSO.Server.Domain;
using RFD.SSO.Server.Model;
using RFD.SSO.Server.Util;

namespace RFD.SSO.Server.Dao
{
    public class UserDao : IUserDao
    {
        public DataTable UserLogIn(Employee employee)
        {
            //根据职工号、员工密码、删除标志查询该员工是否存在。
            string sql = @" SELECT  e.EmployeeID ,
                                    e.EmployeeCode ,
                                    e.EmployeeName ,
                                    e.StationID ,
                                    c.companyname ,
                                    e.DistributionCode ,
                                    e.SysManager,
                                    e.PassWord
                            FROM    employee e ( NOLOCK ) ,
                                    ExpressCompany c ( NOLOCK ) ,
                                    Distribution d ( NOLOCK )
                            WHERE   e.StationID = c.expressCompanyid
                                    AND e.DistributionCode = d.DistributionCode
                                    AND e.EmployeeCode = @EmployeeCode
                                    AND e.IsDeleted = 0
                                    AND d.isdelete = 0  ";

            SqlParameter sqlParam = new SqlParameter("@EmployeeCode", SqlDbType.NVarChar) { Value = employee.EmployeeCode };
            var ds = SqlHelper.ExecuteDataset(ConnectString.PmsReadOnlyConStr, CommandType.Text, sql, sqlParam);

            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count < 1)
            {
                throw new SsoException("用户名或密码错误");
            }

            if (ds.Tables[0].Rows.Count > 1)
            {
                throw new SsoException("用户记录数不唯一,请联系管理员");
            }

            if (MD5.Encrypt(employee.PassWord) != ds.Tables[0].Rows[0]["PassWord"].ToString())
            {
                throw new SsoException("用户名或密码错误");
            }

            return ds.Tables[0];
        }


        /// <summary>
        /// 根据员工编号获取菜单
        /// </summary>
        /// <param name="UserID">员工编号</param>
        /// <returns></returns>
        public DataSet GetMenuListByUserID(string UserID)
        {
            string strSql = @"select distinct m.MenuName,m.URL,m.MenuGroup,m.MenuLevel,m.MainSortBy,m.Sorting ,m.systemID,s.StatusName SystemName from RFD_PMS.dbo.EmployeeRole e(nolock),RFD_PMS.dbo.RoleMenu rm(nolock),RFD_PMS.dbo.Menu m(nolock) 
                     , RFD_PMS.dbo.StatusInfo s(nolock) 
                    where e.RoleID=rm.RoleID and rm.menuID=m.menuID and e.isdeleted=0  
                    and s.statustype='系统名称'and s.StatusNO=cast(m.SystemID as nvarchar(20)) and s.isdelete=0
                    and rm.isdeleted=0 and m.IsDelete=0 and e.employeeID={0} 
                    order by m.MainSortBy,m.Sorting";
            strSql = string.Format(strSql, UserID);
            return SqlHelper.ExecuteDataset(ConnectString.PmsReadOnlyConStr, CommandType.Text, strSql);
        }
    }
}
