using System.Collections.Generic;
using System.ServiceModel;
using System.Data;
using RFD.SSO.Server.Model;

namespace RFD.SSO.Server.Service
{
    // 注意: 如果更改此处的接口名称 "ISSOService"，也必须更新 Web.config 中对 "ISSOService" 的引用。
    [ServiceContract]
    public interface ISSOService
    {
        [OperationContract]
        string Login(LoginRequest loginRequest);

        [OperationContract]
        void Logout(string siteId, string token);

        [OperationContract]
        SsoResponse ValidateToken(string siteId, string ip, string token);

        [OperationContract]
        string GetLoginUrl();

        [OperationContract]
        string GetWebAuthHandler(string siteId);

        [OperationContract]
        List<Navigation> GetNavigationBar();

        [OperationContract]
        DataSet GetMenuListByUserID(string UserID);
    }
}
