using System.Collections.Generic;
using RFD.SSO.Server.Model;

namespace RFD.SSO.Server.Domain
{
    public interface IDB
    {
        bool Add(SsoToken ssoToken);
        bool Update(SsoToken ssoToken);
        bool Remove(string token);
        //Dictionary<string, SsoToken> GetAll();
        SsoToken GetOne(string token);
    }
}
