﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.4963
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System.Runtime.Serialization;


[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
[System.Runtime.Serialization.DataContractAttribute(Name = "LoginRequest", Namespace = "http://schemas.datacontract.org/2004/07/RFD.SSO.Server.Model")]
public partial class LoginRequest : object, System.Runtime.Serialization.IExtensibleDataObject
{

    private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

    private string IPField;

    private string LoginIdField;

    private string PasswordField;

    private string WebSiteField;

    public System.Runtime.Serialization.ExtensionDataObject ExtensionData
    {
        get
        {
            return this.extensionDataField;
        }
        set
        {
            this.extensionDataField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string IP
    {
        get
        {
            return this.IPField;
        }
        set
        {
            this.IPField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string LoginId
    {
        get
        {
            return this.LoginIdField;
        }
        set
        {
            this.LoginIdField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Password
    {
        get
        {
            return this.PasswordField;
        }
        set
        {
            this.PasswordField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string WebSite
    {
        get
        {
            return this.WebSiteField;
        }
        set
        {
            this.WebSiteField = value;
        }
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
[System.Runtime.Serialization.DataContractAttribute(Name = "SsoResponse", Namespace = "http://schemas.datacontract.org/2004/07/RFD.SSO.Server.Model")]
public partial class SsoResponse : object, System.Runtime.Serialization.IExtensibleDataObject
{

    private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

    private string CompanynameField;

    private string DistributionCodeField;

    private string EmployeeCodeField;

    private int EmployeeIDField;

    private string EmployeeNameField;

    private int StationIDField;

    private int SysManagerField;

    public System.Runtime.Serialization.ExtensionDataObject ExtensionData
    {
        get
        {
            return this.extensionDataField;
        }
        set
        {
            this.extensionDataField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Companyname
    {
        get
        {
            return this.CompanynameField;
        }
        set
        {
            this.CompanynameField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string DistributionCode
    {
        get
        {
            return this.DistributionCodeField;
        }
        set
        {
            this.DistributionCodeField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string EmployeeCode
    {
        get
        {
            return this.EmployeeCodeField;
        }
        set
        {
            this.EmployeeCodeField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public int EmployeeID
    {
        get
        {
            return this.EmployeeIDField;
        }
        set
        {
            this.EmployeeIDField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string EmployeeName
    {
        get
        {
            return this.EmployeeNameField;
        }
        set
        {
            this.EmployeeNameField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public int StationID
    {
        get
        {
            return this.StationIDField;
        }
        set
        {
            this.StationIDField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public int SysManager
    {
        get
        {
            return this.SysManagerField;
        }
        set
        {
            this.SysManagerField = value;
        }
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
[System.Runtime.Serialization.DataContractAttribute(Name = "Navigation", Namespace = "http://schemas.datacontract.org/2004/07/RFD.SSO.Server.Model")]
public partial class Navigation : object, System.Runtime.Serialization.IExtensibleDataObject
{

    private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

    private string SiteNameField;

    private string WebAuthHandlerField;

    public System.Runtime.Serialization.ExtensionDataObject ExtensionData
    {
        get
        {
            return this.extensionDataField;
        }
        set
        {
            this.extensionDataField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string SiteName
    {
        get
        {
            return this.SiteNameField;
        }
        set
        {
            this.SiteNameField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string WebAuthHandler
    {
        get
        {
            return this.WebAuthHandlerField;
        }
        set
        {
            this.WebAuthHandlerField = value;
        }
    }
}



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName = "ISSOService")]
public interface ISSOService
{

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ISSOService/Login", ReplyAction = "http://tempuri.org/ISSOService/LoginResponse")]
    string Login(LoginRequest loginRequest);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ISSOService/Logout", ReplyAction = "http://tempuri.org/ISSOService/LogoutResponse")]
    void Logout(string siteId, string token);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ISSOService/ValidateToken", ReplyAction = "http://tempuri.org/ISSOService/ValidateTokenResponse")]
    SsoResponse ValidateToken(string siteId, string ip, string token);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ISSOService/GetLoginUrl", ReplyAction = "http://tempuri.org/ISSOService/GetLoginUrlResponse")]
    string GetLoginUrl();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ISSOService/GetWebAuthHandler", ReplyAction = "http://tempuri.org/ISSOService/GetWebAuthHandlerResponse")]
    string GetWebAuthHandler(string siteId);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ISSOService/GetNavigationBar", ReplyAction = "http://tempuri.org/ISSOService/GetNavigationBarResponse")]
    Navigation[] GetNavigationBar();
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface ISSOServiceChannel : ISSOService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class SSOServiceClient : System.ServiceModel.ClientBase<ISSOService>, ISSOService
{

    public SSOServiceClient()
    {
    }

    public SSOServiceClient(string endpointConfigurationName) :
        base(endpointConfigurationName)
    {
    }

    public SSOServiceClient(string endpointConfigurationName, string remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public SSOServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public SSOServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
        base(binding, remoteAddress)
    {
    }

    public string Login(LoginRequest loginRequest)
    {
        return base.Channel.Login(loginRequest);
    }

    public void Logout(string siteId, string token)
    {
        base.Channel.Logout(siteId, token);
    }

    public SsoResponse ValidateToken(string siteId, string ip, string token)
    {
        return base.Channel.ValidateToken(siteId, ip, token);
    }

    public string GetLoginUrl()
    {
        return base.Channel.GetLoginUrl();
    }

    public string GetWebAuthHandler(string siteId)
    {
        return base.Channel.GetWebAuthHandler(siteId);
    }

    public Navigation[] GetNavigationBar()
    {
        return base.Channel.GetNavigationBar();
    }
}