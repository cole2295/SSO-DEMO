using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB;
using RFD.SSO.Server.Domain;
using RFD.SSO.Server.Model;
using RFD.SSO.Server.Service;
using RFD.SSO.Server.ServiceImpl;


namespace RFD.SSO.Server.Dao
{
    public class MongoDb : IDB
    {
        private readonly string _connectionString;
        private readonly string _dbName;

        public MongoDb()
        {
            _connectionString = MyConfigurationManager.Instance.AppSettings("MongoDb.config")["connectionString"];
            _dbName = MyConfigurationManager.Instance.AppSettings("MongoDb.config")["dbname"];
        }

        public bool Add(SsoToken ssoToken)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();// 打开连接
                var db = mongo.GetDatabase(_dbName);// 切换到指定的数据库
                var collection = db.GetCollection<SsoToken>();// 根据类型获取相应的集合
                //访问collection，做你想做的操作
                //collection.Remove(x => x.Token == ssoToken.Token);
                collection.Insert(ssoToken);
            }
            return true;
        }

        public bool Update(SsoToken ssoToken)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();// 打开连接
                var db = mongo.GetDatabase(_dbName);// 切换到指定的数据库
                var collection = db.GetCollection<SsoToken>();// 根据类型获取相应的集合
                //访问collection，做你想做的操作
                collection.Update(ssoToken, (x => x.Token == ssoToken.Token), true);
            }
            return true;
        }

        public bool Remove(string token)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();// 打开连接
                var db = mongo.GetDatabase(_dbName);// 切换到指定的数据库
                var collection = db.GetCollection<SsoToken>();// 根据类型获取相应的集合
                //访问collection，做你想做的操作
                collection.Remove(x => x.Token == token);
            }
            return true;
        }

        public Dictionary<string, SsoToken> GetAll()
        {
            Dictionary<string, SsoToken> r = new Dictionary<string, SsoToken>();

            List<SsoToken> list = new List<SsoToken>();

            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();// 打开连接
                var db = mongo.GetDatabase(_dbName);// 切换到指定的数据库
                list = db.GetCollection<SsoToken>().Linq().ToList();
            }

            foreach (var v in list)
            {
                if (!r.ContainsKey(v.Token))
                {
                    r.Add(v.Token, v);
                }
            }

            return r;
        }


        public SsoToken GetOne(string token)
        {
            SsoToken st;
            GetAll().TryGetValue(token, out st);
            return st;
        }
    }

    //public class MongoDb2 : IDB
    //{
    //    public bool Add(SsoToken ssoToken)
    //    {
    //        Vancl.NoSQL.MongoDbServices.Interface.IMongo imongo =
    //            new Vancl.NoSQL.MongoDbServices.ServicesImp.MongoHelper("mongodb://127.0.0.1:27017/RfdSso");

    //        imongo.InsertOne<SsoToken>("test2", ssoToken);
    //        return true;
    //    }

    //    public bool Remove(string token)
    //    {
    //        throw new NotImplementedException();
    //        //Vancl.NoSQL.MongoDbServices.Interface.IMongo imongo =
    //        //      new Vancl.NoSQL.MongoDbServices.ServicesImp.MongoHelper("mongodb://127.0.0.1:27017/RfdSso");

    //        //imongo.Delete()
    //    }

    //    public Dictionary<string, SsoToken> GetAll()
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public bool Update(SsoToken ssoToken)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

}
