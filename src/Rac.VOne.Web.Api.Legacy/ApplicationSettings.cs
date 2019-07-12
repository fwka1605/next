using System;
using System.Configuration;
using System.Linq;
using Rac.VOne.Common;
using Rac.VOne.Data;

namespace Rac.VOne.Web.Api.Legacy
{
    /// <summary>web api の設定</summary>
    public class ApplicationSettings : ISetting, IConnectionString, ITimeOutSetter
    {
        /// <summary>
        /// 設定値の取得
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSetting(string key)
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                throw new ApplicationException($"The key could not found: {key}");
            }
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// タイムアウトの取得
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeOut()
        {
            var value = ConfigurationManager.AppSettings["TransactionTimeoutSecond"];
            var second = 0;
            if (int.TryParse(value, out second)) return TimeSpan.FromSeconds(second);
            throw new ArgumentException($"TransactionTimeoutSecound is not setted.");
        }

        /// <summary>
        /// 接続文字列の取得
        /// </summary>
        public string ConnectionString
        {
            get {
                var settings = ConfigurationManager.ConnectionStrings["VOne.Scarlet"];
                return settings?.ConnectionString;
            }
        }

    }
}
