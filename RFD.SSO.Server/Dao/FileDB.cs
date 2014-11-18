using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using RFD.SSO.Server.Domain;
using RFD.SSO.Server.Model;

namespace RFD.SSO.Server.Dao
{
    public class FileDB : IDB
    {
        private readonly string _path = "SsoDB";

        public bool Add(SsoToken ssoToken)
        {
            IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化对象

            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, ssoToken);//把SsoToken对象序列化到内存流
                byte[] buffer = ms.ToArray();//把内存流对象写入字节数组

                string fileFullName = string.Format(@"{0}\{1}.ssodb", _path, ssoToken.Token);

                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }

                //if (File.Exists(fileFullName))
                //{
                //    File.Delete(fileFullName);
                //}

                using (FileStream fs = new FileStream(fileFullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    fs.Write(buffer, 0, buffer.Length);
                }
            }

            return true;
        }

        public bool Remove(string token)
        {
            string fileFullName = string.Format(@"{0}\{1}.ssodb", _path, token);

            if (File.Exists(fileFullName))
            {
                File.Delete(fileFullName);
            }
            return true;
        }

        public Dictionary<string, SsoToken> GetAll()
        {
            var list = new Dictionary<string, SsoToken>();
            var bFormatter = new BinaryFormatter();

            foreach (var dir in Directory.GetFiles(_path, "*.ssodb"))
            {
                using (var fs = File.OpenRead(dir))
                {
                    if (!list.ContainsKey(Path.GetFileNameWithoutExtension(dir)))
                    {
                        list.Add(Path.GetFileNameWithoutExtension(dir), (bFormatter.Deserialize(fs) as SsoToken));
                    }
                }
            }

            return list;
        }

        public bool Update(SsoToken ssoToken)
        {
            throw new System.NotImplementedException();
        }


        public SsoToken GetOne(string token)
        {
            SsoToken st;
            GetAll().TryGetValue(token, out st);
            return st;
        }
    }
}
