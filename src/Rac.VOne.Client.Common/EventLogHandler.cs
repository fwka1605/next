using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common
{
    class EventLogHandler
    {
        public static string SourceName { get; set; }
        private const int MaxMessageLength = 31839;
        public static void WriteSimpleLog(string message, EventLogEntryType tp = EventLogEntryType.Information)
        {
            var msg = message;
            if (msg.Length > MaxMessageLength) msg = msg.Substring(0, MaxMessageLength);
            try
            {
                EventLog.WriteEntry(SourceName, msg, tp);
            }
            catch (Exception)
            {
            }
        }
    }
}
