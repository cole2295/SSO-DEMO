using System;

namespace RFD.SSO.Server.Service
{
    /// <summary>
    /// 隔离系统时间，方便测试
    /// </summary>
    public class ApiDateTime
    {
        private static ApiDateTime _instance;
        private DateTime? _now = null;

        private ApiDateTime()
        {
        }

        public static ApiDateTime Instance
        {
            get
            {
                return _instance ?? (_instance = new ApiDateTime());
            }
        }

        /// <summary>
        /// 设定当前时间
        /// </summary>
        /// <param name="setNow"></param>
        public void SetNow(DateTime? setNow)
        {
            _now = setNow;
        }

        public DateTime Now
        {
            get
            {
                return _now ?? DateTime.Now;
            }
        }
    }
}
