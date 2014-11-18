using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB;

namespace RFD.SSO.Server.Ado
{
    public class MongoDbHelper
    {
        private const string ConnectionString = "Server=127.0.0.1";
        private const string DbName = "RfdSso";

        public void Insert<T>(T document) where T : class
        {
            // 首先创建一个连接
            using (Mongo mongo = new Mongo(ConnectionString))
            {
                // 打开连接
                mongo.Connect();

                // 切换到指定的数据库
                var db = mongo.GetDatabase(DbName);

                // 根据类型获取相应的集合
                var collection = db.GetCollection<T>();
                // 【访问collection，做你想做的操作】

                collection.Insert(document);
            }
        }


        public void Update<T>(T document) where T : class
        {
            using (Mongo mongo = new Mongo(ConnectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(DbName);
                var collection = db.GetCollection<T>();

                //collection
            }
        }
    }
}
