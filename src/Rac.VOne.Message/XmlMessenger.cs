using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Rac.VOne.Message
{
    public class XmlMessenger
    {
        protected DataTable MessageTable;
        private const string ErrorNonMessage = "MessageID 「{0}」が存在しません。";
        private const string xmlFileName = "message.xml";

        public XmlMessenger()
        {
            var dt = GetPatternDataFromXml(xmlFileName);
            Initialize(xmlFileName, dt);
        }

        public XmlMessenger(string filePath)
        {
            var xmlReader = new XmlTextReader(filePath);
            var ds = new DataSet();
            ds.ReadXml(xmlReader);
            Initialize(filePath, ds.Tables[0]);
        }


        private DataTable GetPatternDataFromXml(string xmlResFileName)
        {
            var dt = new DataTable();
            try
            {
                var assem = typeof(XmlMessenger).Assembly;
                var xmlResName = string.Format("{0}.{1}", assem.GetName().Name, xmlResFileName);
                string xmlStr;
                using (var reader = new StreamReader(assem.GetManifestResourceStream(xmlResName), Encoding.UTF8))
                {
                    xmlStr = reader.ReadToEnd();
                }
                using (var sr = new StringReader(xmlStr))
                {
                    var ds = new DataSet();
                    ds.ReadXml(sr);
                    dt = ds.Tables[0];
                }
                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public void Initialize(string resName, DataTable dt)
        {
            var syncObject = new object();
            lock(syncObject)
            {
                MessageTable = dt;
            }
        }

        public MessageInfo GetMessageInfo(string id, params string[] args)
        {
            MessageInfo info = null;
            if (MessageTable == null) return info;
            var messageRows = MessageTable.Select(string.Format("MessageID='{0}'", id));

            if (messageRows.Length == 0) throw new ArgumentException(string.Format(ErrorNonMessage, id));

            info = new MessageInfo() { ID = id };
            var row = messageRows[0];
            var ColumnText = "MessageInfo";
            var ColumnTitle = "MessageTitle";
            var ColumnButtons = "MessageButton";
            var ColumnCategory = "MessageCategory";

            if (!row.IsNull(ColumnText)) info.Text = string.Format(row.Field<string>(ColumnText), args);
            info.Title = row.Field<string>(ColumnTitle);
            int category;
            if (!row.IsNull(ColumnCategory) && int.TryParse(row[ColumnCategory].ToString(), out category) && Enum.IsDefined(typeof(MessageCategory), category))
            {
                info.Category = (MessageCategory)Enum.ToObject(typeof(MessageCategory), category);
            }
            else
            {
                info.Category = MessageCategory.Information;
            }

            int button;
            if (!row.IsNull(ColumnButtons) && int.TryParse(row[ColumnButtons].ToString(), out button)) info.Buttons = (MessageBoxButtons)button;
            return info;
        }
    }
}
