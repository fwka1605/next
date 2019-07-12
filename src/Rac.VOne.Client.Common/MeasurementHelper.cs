using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common
{
    public class MeasurementHelper
    {
        private static DateTime? StartTime { get; set; }
        private static bool IsInitialize { get; set; }
        private const string ApplicationName = "VOneG4";

        private static bool IsNeedMeasurement()
        {
            var val = System.Configuration.ConfigurationManager.AppSettings["IsNeedMeasurement"];
            return (val != null && val.ToLower().Equals("true"));
        }

        private static void Initialize()
        {
            if (!IsInitialize)
            {
                EventLogHandler.SourceName = ApplicationName;
                IsInitialize = true;
            }
        }

        public static void ProcessStart()
        {
            if (!IsNeedMeasurement()) return;
            StartTime = DateTime.Now;
        }

        public static void ProcessEnd(string processName)
        {
            if (!IsNeedMeasurement() || StartTime == null) return;

            Initialize();

            var message = new System.Text.StringBuilder();
            var EndTime = DateTime.Now;
            message.AppendLine(processName);

            var ts = EndTime - StartTime.Value;
            message.AppendFormat("{0:#,##0}", ts.TotalMilliseconds);

            EventLogHandler.WriteSimpleLog(message.ToString());

            StartTime = null;
        }
    }
}
