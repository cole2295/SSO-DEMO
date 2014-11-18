using LMS.Util.Security;

namespace RFD.SSO.Server.Service
{
    public interface ICryptography
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainString"></param>
        /// <param name="cryptographyKey"></param>
        /// <returns></returns>
        string Encrypt(string plainString, string cryptographyKey);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <param name="cryptographyKey"></param>
        /// <returns></returns>
        string Decrypt(string encryptedString, string cryptographyKey);
    }


    public class DSA : ICryptography
    {
        #region ICryptography 成员

        public string Encrypt(string plainString, string cryptographyKey)
        {
            return DES.Encrypt3DES(plainString);
        }

        public string Decrypt(string encryptedString, string cryptographyKey)
        {
            return DES.Decrypt3DES(encryptedString);
        }

        #endregion
    }



}
