using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class CustomerGroupFileDefinition : RowDefinition<CustomerGroup>
    {
        public StandardIdToCodeFieldDefinition<CustomerGroup, Company> CompanyIdField { get; private set; }
        public StandardIdToCodeFieldDefinition<CustomerGroup, Customer> ChildCustomerField { get; private set; }
        public StandardIdToCodeFieldDefinition<CustomerGroup, Customer> ParentCustomerField { get; private set; }

        public CustomerGroupFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "債権代表者";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<CustomerGroup, Company>(
                    null, null, //k => k.CompanyId, c => c.Id,
                    null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };
            ParentCustomerField = new StandardIdToCodeFieldDefinition<CustomerGroup, Customer>(
                k => k.ParentCustomerId, c => c.Id,
                null, c => c.Code)
            {
                FieldName = "債権代表者コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitCustomerGroupCode,
                GetModelsByCode = val => GetCustomerDictionary(val),
            };
            ChildCustomerField = new StandardIdToCodeFieldDefinition<CustomerGroup, Customer>(
                  k => k.ChildCustomerId, c => c.Id,
                  null, c => c.Code)
            {
                FieldName = "得意先コード",
                FieldNumber = 3,
                Required = true,
                Accept = VisitCustomerCode,
                GetModelsByCode = val => GetCustomerDictionary(val),
                ValidateAdditional = ValidateAdditionalChildCustomerField,
            };
            Fields.AddRange(new IFieldDefinition<CustomerGroup>[] {
                    CompanyIdField, ParentCustomerField, ChildCustomerField});
            KeyFields.AddRange(new IFieldDefinition<CustomerGroup>[]
            {
                ParentCustomerField,
                ChildCustomerField,
            });
        }

        private bool VisitCompanyId(IFieldVisitor<CustomerGroup> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitCustomerGroupCode(IFieldVisitor<CustomerGroup> visitor)
        {
            return visitor.CustomerCode(ParentCustomerField);
        }

        private bool VisitCustomerCode(IFieldVisitor<CustomerGroup> visitor)
        {
            return visitor.CustomerCode(ChildCustomerField);
        }
        public Func<List<CustomerGroup>> GetDbCsutomerGroups { private get; set; }
        public Func<string[], Dictionary<string, Customer>> GetCustomerDictionary { private get; set; }

        /// <summary>
        /// 重複確認 <see cref="SortedDictionary{TKey, TValue}"/> を利用できるようなメソッド
        /// </summary>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private bool HasDupedItem(SortedList<int, CustomerGroup[]> list,
            int key, Func<CustomerGroup[], bool> condition)
            => list.ContainsKey(key) && condition(list[key]);
        private IEnumerable<WorkingReport> ValidateAdditionalChildCustomerField(Dictionary<int, CustomerGroup> val, object param)
        {
            var reports = new List<WorkingReport>();
            var dbItems = GetDbCsutomerGroups();

            var dbParentCheckList = new SortedList<int, CustomerGroup[]>(dbItems.GroupBy(x => x.ParentCustomerId)
                .ToDictionary(x => x.Key, x => x.ToArray()));
            var dbChildCheckList = new SortedList<int, CustomerGroup[]>(dbItems.GroupBy(x => x.ChildCustomerId)
                .ToDictionary(x => x.Key, x => x.ToArray()));
            var csvParentCheckList = new SortedList<int, CustomerGroup[]>(val.Select(x => x.Value)
                .GroupBy(x => x.ParentCustomerId)
                .ToDictionary(x => x.Key, x => x.ToArray()));
            var csvChildCheckList = new SortedList<int, CustomerGroup[]>(val.Select(x => x.Value)
                .GroupBy(x => x.ChildCustomerId)
                .ToDictionary(x => x.Key, x => x.ToArray()));

            foreach (KeyValuePair<int, CustomerGroup> pair in val)
            {
                //データベースとの比較検証

                //親得意先が既に他の債権代表者グループの子得意先として登録されている場合、エラーとする
                if (HasDupedItem(dbChildCheckList,
                    pair.Value.ParentCustomerId,
                    g => g.Any(x => x.ParentCustomerId != pair.Value.ParentCustomerId)))
                {
                    reports.Add(new WorkingReport(pair.Key,
                        ParentCustomerField.FieldIndex,
                        ParentCustomerField.FieldName,
                        "他グループの得意先コードとして登録されているため、インポートできません。"));
                }
                //子得意先が既に他の債権代表者グループの子得意先として登録されている場合、エラーとする
                if (HasDupedItem(dbChildCheckList,
                    pair.Value.ChildCustomerId,
                    g => g.Any(x => x.ParentCustomerId != pair.Value.ParentCustomerId)))
                {
                    reports.Add(new WorkingReport(pair.Key,
                        ChildCustomerField.FieldIndex,
                        ChildCustomerField.FieldName,
                        "他グループの得意先コードとして登録されているため、インポートできません。"));
                }
                //子得意先が既に他の債権代表者グループの親得意先として登録されている場合、エラーとする
                if (HasDupedItem(dbParentCheckList,
                    pair.Value.ChildCustomerId,
                    g => g.Any(x => x.ChildCustomerId != pair.Value.ChildCustomerId)))
                {
                    reports.Add(new WorkingReport(pair.Key,
                        ChildCustomerField.FieldIndex,
                        ChildCustomerField.FieldName,
                        "他グループの債権代表者コードとして登録されているため、インポートできません。"));
                }

                //CSVファイル内での比較検証
                if (pair.Value.ParentCustomerId == pair.Value.ChildCustomerId)
                {
                    reports.Add(new WorkingReport(pair.Key,
                        ChildCustomerField.FieldIndex,
                        ChildCustomerField.FieldName,
                        "債権代表者コードと同じコードは登録できません。"));
                }

                //親得意先がファイル内で他の債権代表者グループの子得意先に指定されている場合、エラーとする
                if (HasDupedItem(csvChildCheckList,
                    pair.Value.ParentCustomerId,
                    g => g.Any(x => x.ParentCustomerId != pair.Value.ParentCustomerId && pair.Value.ParentCustomerId != 0)))
                {
                    reports.Add(new WorkingReport(pair.Key,
                        ParentCustomerField.FieldIndex,
                        ParentCustomerField.FieldName,
                        "他グループの得意先コードに指定されているため、インポートできません。"));
                }
                //子得意先がファイル内で他の債権代表者グループの子得意先に指定されている場合、エラーとする
                if (HasDupedItem(csvChildCheckList,
                    pair.Value.ChildCustomerId,
                    g => g.Any(x => x.ParentCustomerId != pair.Value.ParentCustomerId && pair.Value.ChildCustomerId != 0)))
                {
                    reports.Add(new WorkingReport(pair.Key,
                        ChildCustomerField.FieldIndex,
                        ChildCustomerField.FieldName,
                        "他グループの得意先コードに指定されているため、インポートできません。"));
                }
                //子得意先がファイル内で他の債権代表者グループの親得意先に指定されている場合、エラーとする
                if (HasDupedItem(csvParentCheckList,
                    pair.Value.ChildCustomerId,
                    g => g.Any(x => x.ChildCustomerId != pair.Value.ChildCustomerId && pair.Value.ChildCustomerId != 0)))
                {
                    reports.Add(new WorkingReport(pair.Key,
                        ChildCustomerField.FieldIndex,
                        ChildCustomerField.FieldName,
                        "他グループの債権代表者コードに指定されているため、インポートできません。"));
                }
            }
            return reports;
        }
    }
}
