using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RFD.SSO.Server.Model
{
    [Serializable]
    [DataContract]
    public class SsoResponse
    {
        [DataMember]
        public int EmployeeID
        {
            get;
            set;
        }

        [DataMember]
        public string EmployeeCode
        {
            get;
            set;
        }

        [DataMember]
        public string EmployeeName
        {
            get;
            set;
        }

        [DataMember]
        public int StationID
        {
            get;
            set;
        }

        [DataMember]
        public string Companyname
        {
            get;
            set;
        }

        [DataMember]
        public string DistributionCode
        {
            get;
            set;
        }

        [DataMember]
        public int SysManager
        {
            get;
            set;
        }
    }
}
