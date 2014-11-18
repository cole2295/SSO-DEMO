using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MongoDB;
using NUnit.Framework;
using RFD.SSO.Server.Dao;
using RFD.SSO.Server.Model;
using RFD.SSO.Server.Service;
using RFD.SSO.Server.ServiceImpl;

namespace RFD.SSO.Server.Test
{
    [TestFixture]
    public class MongoTest
    {
        [Test, ExpectedException]
        public void CanNotInsertDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("aaa");
            dt.Columns.Add("bbb");
            var r = dt.NewRow();
            r["aaa"] = "123";
            r["bbb"] = "333";
            dt.Rows.Add(r);


            using (Mongo mgo = new Mongo("Server=127.0.0.1"))
            {
                mgo.Connect();
                var db = mgo.GetDatabase("sso");

                var collection = db.GetCollection<string>();

                collection.Insert(dt);
            }
        }

        [Test]
        public void a()
        {
            LoginRequest request = new LoginRequest();
            request.IP = "123.1.1.1";
            request.LoginId = "admin";
            request.Password = "1";
            request.WebSite = "www.vancl.com";

            SsoToken s = new SsoToken
            {
                LoginRequest = request,
                LoginTime = ApiDateTime.Instance.Now,
                UserData = null
            };



            using (Mongo mgo = new Mongo("Server=127.0.0.1"))
            {
                mgo.Connect();
                var db = mgo.GetDatabase("RfdSso");

                var collection = db.GetCollection<SsoToken>();

                collection.Insert(s);
            }
        }

        [Test]
        public void add1()
        {

            LoginRequest request = new LoginRequest();
            request.IP = "123.1.1.1";
            request.LoginId = "admin";
            request.Password = "1";
            request.WebSite = "www.vancl.com";

            SsoToken s = new SsoToken
            {
                LoginRequest = request,
                LoginTime = ApiDateTime.Instance.Now,
                UserData = new SsoResponse()
            };


            MongoDb a = new MongoDb();
            Assert.IsTrue(a.Add(s));
        }

        [Test]
        public void DeleteTest()
        {
            MongoDb m = new MongoDb();
            m.Remove("123.1.1.1");
        }


        [Test]
        public void UpdateTest()
        {
            LoginRequest request = new LoginRequest();
            request.IP = "123.1.1.1";
            request.LoginId = "admin";
            request.Password = "1";
            request.WebSite = "www.vancl.com";

            SsoToken s = new SsoToken
            {
                LoginRequest = request,
                LoginTime = ApiDateTime.Instance.Now,
                UserData = new SsoResponse()
            };

            MongoDb a = new MongoDb();
            
            Assert.IsTrue(a.Update(s));
        }

        [Test]
        public void MongoDb2Test()
        {
            LoginRequest request = new LoginRequest();
            request.IP = "123.1.1.1";
            request.LoginId = "admin";
            request.Password = "1";
            request.WebSite = "www.vancl.com";

            SsoToken s = new SsoToken
            {
                LoginRequest = request,
                LoginTime = ApiDateTime.Instance.Now,
                UserData = new SsoResponse()
            };


            //MongoDb2 a = new MongoDb2();
            //a.Add(s);
        }
    }

    public class User
    {
        public string UserId
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
    }
}
