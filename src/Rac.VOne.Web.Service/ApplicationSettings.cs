using System;
using System.Configuration;
using System.Linq;
using Rac.VOne.Common;
using Rac.VOne.Data;

namespace Rac.VOne.Web.Service
{
    public class ApplicationSettings : ISetting, IConnectionString, ITimeOutSetter
    {
        public string GetSetting(string key)
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                throw new ApplicationException($"The key could not found: {key}");
            }
            return ConfigurationManager.AppSettings[key];
        }

        public string ConnectionString
        {
            get {
                var settings = ConfigurationManager.ConnectionStrings["VOne.Scarlet"];
                return settings?.ConnectionString;
            }
        }

        public TimeSpan GetTimeOut()
        {
            var value = ConfigurationManager.AppSettings["TransactionTimeoutSecond"];
            var second = 0;
            if (int.TryParse(value, out second)) return TimeSpan.FromSeconds(second);
            throw new ArgumentException($"TransactionTimeoutSecound is not setted.");
        }

    }
}
