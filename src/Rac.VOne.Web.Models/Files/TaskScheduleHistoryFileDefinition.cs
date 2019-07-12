using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class TaskScheduleHistoryFileDefinition : RowDefinition<TaskScheduleHistory>
    {
        public NumberFieldDefinition<TaskScheduleHistory, DateTime> StartAtField { get; private set; }
        public NumberFieldDefinition<TaskScheduleHistory, DateTime> EndAtField { get; private set; }
        public NumberFieldDefinition<TaskScheduleHistory, int> ImportTypeField { get; private set; }
        public NumberFieldDefinition<TaskScheduleHistory, int> ImportSubTypeField { get; private set; }
        public NumberFieldDefinition<TaskScheduleHistory, int> ResultField { get; private set; }
        public StringFieldDefinition<TaskScheduleHistory> ErrorsField { get; private set; }

        // ===> 表示値変換用
        public Dictionary<int, Dictionary<int, string>> ImportTypeToImportSubType { get; set; }
        public Dictionary<int, string> ImportType { get; set; }
        public Dictionary<int, string> ImportSubType0 { get; set; }
        public Dictionary<int, string> ImportSubType1 { get; set; }
        public Dictionary<int, string> ImportSubType2 { get; set; }
        public Dictionary<int, string> ImportSubType3 { get; set; }
        public Dictionary<int, string> ImportSubType4 { get; set; }
        public Dictionary<int, string> ImportSubType5 { get; set; }
        public Dictionary<int, string> Result { get; set; }

        /// <summary>ImportSubTypeFieldの変換時にImportTypeも使うので保持しておく用</summary>
        private int currentImportType;
        // <===

        public TaskScheduleHistoryFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "タイムスケジューラーログ";
            FileNameToken = DataTypeToken;

            StartAtField = new NumberFieldDefinition<TaskScheduleHistory, DateTime>(h => h.StartAt)
            {
                FieldName = "開始日時",
                FieldNumber = 1,
                Required = true,
                Accept = VisitStartAt,
                Format = value => value.ToString(),
            };

            EndAtField = new NumberFieldDefinition<TaskScheduleHistory, DateTime>(h => h.EndAt)
            {
                FieldName = "終了日時",
                FieldNumber = 2,
                Required = true,
                Accept = VisitEndAt,
                Format = value => value.ToString(),
            };

            ImportTypeField = new NumberFieldDefinition<TaskScheduleHistory, int>(h => h.ImportType)
            {
                FieldName = "種別",
                FieldNumber = 3,
                Required = true,
                Accept = VisitImportType,
                Format = value => { /* ★ */currentImportType = value; return ImportType[value]; },
            };

            ImportSubTypeField = new NumberFieldDefinition<TaskScheduleHistory, int>(h => h.ImportSubType)
            {
                FieldName = "取込パターン",
                FieldNumber = 4,
                Required = true,
                Accept = VisitImportSubType,
                Format = value => ImportTypeToImportSubType[/* ★ */currentImportType][value],
            };

            ResultField = new NumberFieldDefinition<TaskScheduleHistory, int>(h => h.Result)
            {
                FieldName = "実行結果",
                FieldNumber = 5,
                Required = true,
                Accept = VisitResult,
                Format = value => Result[value],
            };

            ErrorsField = new StringFieldDefinition<TaskScheduleHistory>(h => h.Errors)
            {
                FieldName = "エラーログ",
                FieldNumber = 6,
                Required = false,
                Accept = VisitErrors,
            };

            Fields.AddRange(new IFieldDefinition<TaskScheduleHistory>[] {
                StartAtField,
                EndAtField,
                ImportTypeField,
                ImportSubTypeField, // 追加順序をImportTypeFieldの後にする必要あり
                ResultField,
                ErrorsField,
            });
        }

        private bool VisitStartAt(IFieldVisitor<TaskScheduleHistory> visitor)
        {
            return visitor.StandardNumber(StartAtField);
        }
        private bool VisitEndAt(IFieldVisitor<TaskScheduleHistory> visitor)
        {
            return visitor.StandardNumber(EndAtField);
        }
        private bool VisitImportType(IFieldVisitor<TaskScheduleHistory> visitor)
        {
            return visitor.StandardNumber(ImportTypeField);
        }
        private bool VisitImportSubType(IFieldVisitor<TaskScheduleHistory> visitor)
        {
            return visitor.StandardNumber(ImportSubTypeField);
        }
        private bool VisitResult(IFieldVisitor<TaskScheduleHistory> visitor)
        {
            return visitor.StandardNumber(ResultField);
        }
        private bool VisitErrors(IFieldVisitor<TaskScheduleHistory> visitor)
        {
            return visitor.StandardString(ErrorsField);
        }
    }
}
