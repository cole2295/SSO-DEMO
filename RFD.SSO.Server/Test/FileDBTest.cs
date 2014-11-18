using System.Data;
using System.IO;
using NUnit.Framework;
using RFD.SSO.Server.Dao;
using RFD.SSO.Server.Model;
using System.Collections.Generic;

namespace RFD.SSO.Server.Test
{
    [TestFixture]
    public class FileDBTest
    {
        [Test]
        public void AddTest()
        {
            var dtt = new SsoResponse
            {
                EmployeeCode = "1"
            };


            //DataTable dtt = new DataTable();
            //dtt.Columns.Add("EmployeeCode");
            //var dr = dtt.NewRow();
            //dr["EmployeeCode"] = "1";
            //dtt.Rows.Add(dr);

            SsoToken ssoToken = new SsoToken
                                    {
                                        LoginRequest = new LoginRequest
                                        {
                                            IP = "127.0.0.1",
                                            LoginId = "abc",
                                            Password = "123",
                                            WebSite = "1"
                                        },
                                        UserData = dtt
                                    };

            FileDB fileDb = new FileDB();

            fileDb.Add(ssoToken);

            string fileFullName = string.Format(@"{0}\{1}.ssodb", "SsoDB", ssoToken.Token);
            Assert.IsTrue(File.Exists(fileFullName));

            SsoToken ssoToken2;
            fileDb.GetAll().TryGetValue(ssoToken.Token, out ssoToken2);

            Assert.AreEqual("1", ssoToken2.UserData.EmployeeCode);
        }
    }
}
