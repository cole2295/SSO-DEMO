using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using System.Data;
using RFD.SSO.Server.Dao;
using RFD.SSO.Server.Domain;
using RFD.SSO.Server.Model;
using RFD.SSO.Server.Service;
using RFD.SSO.Server.ServiceImpl;


namespace RFD.SSO.Server.Test
{
    [TestFixture]
    public class SsoTest
    {
        private readonly Mock<IUserDao> _mockDao;

        public SsoTest()
        {
            _mockDao = new Mock<IUserDao>();
            _mockDao.Setup(u => u.UserLogIn(It.IsAny<Employee>()))
                .Returns((Employee e) =>
                {
                    if (e.EmployeeCode == "admin")
                    {
                        DataTable dtt = new DataTable();
                        dtt.Columns.Add("EmployeeID");
                        dtt.Columns.Add("EmployeeCode");
                        dtt.Columns.Add("EmployeeName");
                        dtt.Columns.Add("StationID");
                        dtt.Columns.Add("companyname");
                        dtt.Columns.Add("DistributionCode");
                        dtt.Columns.Add("SysManager");

                        var dr = dtt.NewRow();
                        dr["EmployeeCode"] = "admin";
                        dtt.Rows.Add(dr);
                        return dtt;
                    }
                    return null;
                });

        }

        [Test]
        public void ValidateTokenTest()
        {
            LoginRequest request = new LoginRequest();
            request.IP = "123.1.1.1";
            request.LoginId = "admin";
            request.Password = "1";
            request.WebSite = "www.vancl.com";

            //登录
            ValidUserList.Instance.UserDao = _mockDao.Object;

            //ValidUserList.Instance.UserDao = new UserDao();
            ValidUserList.Instance.DB = new MongoDb();
            string token = ValidUserList.Instance.Login(request);
            Console.WriteLine(token);

            //验证
            SsoResponse dt;
            Assert.AreEqual(true, ValidUserList.Instance.ValidateToken("lms.wuliusys.com", request.IP, token, out dt));
            Assert.IsNotNull(dt);

            //别的机器拿着token来验证
            Assert.AreEqual(false, ValidUserList.Instance.ValidateToken("lms.wuliusys.com", "123.1.1.2", token, out dt));
            Assert.IsTrue(dt == null);

            //登录机器再验证
            Assert.AreEqual(true, ValidUserList.Instance.ValidateToken("lms.wuliusys.com", "123.1.1.1", token, out dt));
            Assert.IsNotNull(dt);

            //站点2再验证
            Assert.AreEqual(true, ValidUserList.Instance.ValidateToken("fms.wuliusys.com", "123.1.1.1", token, out dt));
            Assert.IsNotNull(dt);

            //退出
            ValidUserList.Instance.Logout("lms.wuliusys.com", token);
            Assert.AreEqual(false, ValidUserList.Instance.ValidateToken("lms.wuliusys.com", "123.1.1.1", token, out dt));
            Assert.IsTrue(dt == null);
        }

        [Test]
        public void ValidateTokenTimeOverTest()
        {
            SsoResponse dt;
            LoginRequest request = new LoginRequest();
            request.IP = "123.1.1.1";
            request.LoginId = "admin";
            request.Password = "1";
            request.WebSite = "www.vancl.com";

            //设定当前时间10:00
            ApiDateTime.Instance.SetNow(DateTime.Parse("2011-10-01 10:00"));

            //登录
            ValidUserList.Instance.UserDao = _mockDao.Object;
            ValidUserList.Instance.DB = new MongoDb();
            string token = ValidUserList.Instance.Login(request);
            Console.WriteLine(token);

            //验证,当前时间是10:00
            ApiDateTime.Instance.SetNow(DateTime.Parse("2011-10-01 10:00"));
            Assert.AreEqual(true, ValidUserList.Instance.ValidateToken("lms.wuliusys.com", request.IP, token, out dt));


            //验证,当前时间是10:10
            ApiDateTime.Instance.SetNow(DateTime.Parse("2011-10-01 10:10"));
            Assert.AreEqual(true, ValidUserList.Instance.ValidateToken("lms.wuliusys.com", request.IP, token, out dt));


            //验证,当前时间是10:30
            ApiDateTime.Instance.SetNow(DateTime.Parse("2011-10-01 10:30"));
            Assert.AreEqual(true, ValidUserList.Instance.ValidateToken("lms.wuliusys.com", request.IP, token, out dt));


            //验证,当前时间是10:31,这个不超时
            ApiDateTime.Instance.SetNow(DateTime.Parse("2011-10-01 10:31"));
            Assert.AreEqual(true, ValidUserList.Instance.ValidateToken("lms.wuliusys.com", request.IP, token, out dt));


            //验证,当前时间是11:02,超时了
            ApiDateTime.Instance.SetNow(DateTime.Parse("2011-10-01 11:02"));
            Assert.AreEqual(false, ValidUserList.Instance.ValidateToken("lms.wuliusys.com", request.IP, token, out dt));
        }

        [Test]
        public void SSOServiceTest()
        {
            LoginRequest request = new LoginRequest();
            request.IP = "123.1.1.1";
            request.LoginId = "admin";
            request.Password = "1";
            request.WebSite = "www.vancl.com";

            //登录
            string token;
            SSOService ss = new SSOService();
            token = ss.Login(request);
            Assert.IsTrue(!string.IsNullOrEmpty(token));

            var ds = ss.ValidateToken("lms.wuliusys.com", request.IP, token);
            Assert.IsNotNull(ds);

            ds = ss.ValidateToken("lms.wuliusys.com", request.IP, token);
            Assert.IsNotNull(ds);
        }
    }

    [TestFixture]
    public class SiteListTest
    {
        [Test]
        public void GetSitesFromXmlTest()
        {
            var sits = SiteList.Instance.Sites;

            Assert.IsTrue(sits.Count > 0);
        }

        [Test]
        public void GetSiteTest()
        {
            var sits = SiteList.Instance.GetSiteInfo("lms.wuliusys.com");

            Assert.AreEqual("lms.wuliusys.com", sits.SiteId);
        }
    }

    [TestFixture]
    public class SsoTokeTest
    {
        [Test]
        public void TimeOver()
        {
            //设定当前时间
            DateTime now = DateTime.Parse("1949-10-01 09:00");
            ApiDateTime.Instance.SetNow(now);

            //ValidateTime为空，即第一次登录时
            SsoToken token = new SsoToken();
            Assert.IsFalse(token.TimeOver);

            //上次验证时间是08:29
            token.ValidateTime = DateTime.Parse("1949-10-01 08:29");
            Assert.IsTrue(token.TimeOver);

            //上次验证时间是08:31
            token.ValidateTime = DateTime.Parse("1949-10-01 08:31");
            Assert.IsFalse(token.TimeOver);
        }

        [Test, ExpectedException]
        public void TimeOverValidateTimeAfterNow()
        {
            //设定当前时间
            DateTime now = DateTime.Parse("1949-10-01 09:00");
            ApiDateTime.Instance.SetNow(now);

            //验证时间比当前时间晚
            SsoToken token = new SsoToken();
            token.ValidateTime = DateTime.Parse("1949-10-01 09:00:01");

            Assert.IsFalse(token.TimeOver);
        }
    }

    [TestFixture]
    public class ApiDateTimeTest
    {
        [Test]
        public void NowTest()
        {
            ApiDateTime.Instance.SetNow(null);
            Assert.IsTrue(ApiDateTime.Instance.Now == DateTime.Now);

            DateTime setDt = DateTime.Parse("1949-10-01");
            ApiDateTime.Instance.SetNow(setDt);

            Assert.AreEqual(setDt, ApiDateTime.Instance.Now);
        }

      

    }
}
