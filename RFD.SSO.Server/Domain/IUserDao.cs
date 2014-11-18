using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using RFD.SSO.Server.Model;

namespace RFD.SSO.Server.Domain
{
    public interface IUserDao
    {
        DataTable UserLogIn(Employee employee);
        DataSet GetMenuListByUserID(string UserID);
    }
}
