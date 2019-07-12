using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    public class LogDataFileDefinition : RowDefinition<LogData>
    {
        public NumberFieldDefinition<LogData, DateTime> LoggedAtField { get; private set; }
        public StringFieldDefinition<LogData> LoginUserCodeField { get; private set; }
        public StringFieldDefinition<LogData> LoginUserNameField { get; private set; }
        public StringFieldDefinition<LogData> MenuNameField { get; private set; }
        public StringFieldDefinition<LogData> OperationNameField { get; private set; }

        public LogDataFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "操作ログ管理";
            FileNameToken = DataTypeToken;

            LoggedAtField = new NumberFieldDefinition<LogData, DateTime>(k => k.LoggedAt)
            {
                FieldName = "日時",
                FieldNumber = 1,
                Required = false,
                Accept = VisitLoggedAt,
                Format = value => value.ToString(),
            };

            LoginUserCodeField = new StringFieldDefinition<LogData>(k => k.LoginUserCode)
            {
                FieldName = "ユーザーコード",
                FieldNumber = 2,
                Required = false,
                Accept = VisitLoginUserCode,
            };

            LoginUserNameField = new StringFieldDefinition<LogData>(k => k.LoginUserName)
            {
                FieldName = "ユーザー名",
                FieldNumber = 3,
                Required = false,
                Accept = VisitLoginUserName,
            };
            MenuNameField = new StringFieldDefinition<LogData>(k => k.MenuName)
            {
                FieldName = "名称",
                FieldNumber = 4,
                Required = false,
                Accept = VisitMenuName,
            };

            OperationNameField = new StringFieldDefinition<LogData>(k => k.OperationName)
            {
                FieldName = "操作",
                FieldNumber = 5,
                Required = false,
                Accept = VisitOperationName,
            };

            Fields.AddRange(new IFieldDefinition<LogData>[] {
                        LoggedAtField, LoginUserCodeField, LoginUserNameField,MenuNameField,OperationNameField});
        }

        private bool VisitLoggedAt(IFieldVisitor<LogData> visitor)
        {
            return visitor.StandardNumber<DateTime>(LoggedAtField);
        }

        private bool VisitLoginUserCode(IFieldVisitor<LogData> visitor)
        {
            return visitor.OwnLoginUserCode(LoginUserCodeField);
        }

        private bool VisitLoginUserName(IFieldVisitor<LogData> visitor)
        {
            return visitor.OwnLoginUserName(LoginUserNameField);
        }

        private bool VisitMenuName(IFieldVisitor<LogData> visitor)
        {
            return visitor.StandardString(MenuNameField);
        }

        private bool VisitOperationName(IFieldVisitor<LogData> visitor)
        {
            return visitor.StandardString(OperationNameField);
        }

    }
}
