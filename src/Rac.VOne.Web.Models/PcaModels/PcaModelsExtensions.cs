using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    public static class PcaModelsExtensions
    {
        /// <summary>仕訳変換処理</summary>
        /// <param name="source">VONE標準仕訳</param>
        /// <param name="dicKmk">PCA 科目マスター</param>
        /// <param name="dicBu"> PCA 部門マスター</param>
        /// <param name="dicHojo">PCA 補助科目マスター</param>
        /// <param name="requireAllDepartment">部門管理：全科目必須</param>
        /// <param name="requirePLDepartment">部門管理：PL科目のみ必須</param>
        /// <param name="errorLogger">変換エラーなどのハンドラ</param>
        /// <returns></returns>
        /// <remarks>
        /// 借方/貸方 税計算区分
        /// 消込仕訳では 税区分が1つしかない
        /// </remarks>
        public static List<BEInputSlip> Convert(
            this List<Rac.VOne.Web.Models.MatchingJournalizing> source,
            Dictionary<string, BEKmk>    dicKmk,
            Dictionary<string, BEBu>     dicBu,
            SortedList<string, BEHojo[]> dicHojo,
            bool requireAllDepartment,
            bool requirePLDepartment,
            Action<string> errorLogger
            )
        {
            var list = new List<BEInputSlip>();
            const string CommonBuCode = "000";
            foreach (var x in source)
            {
                var valid = true;
                var data   = new BEInputSlipData();
                var commonBu = dicBu.GetValue(CommonBuCode);

                var dbKmk  = dicKmk .GetValue(x.DebitAccountTitleCode);
                var dbBu   = (requireAllDepartment
                           || requirePLDepartment && (dbKmk?.IsPLAccount ?? false))
                           ? dicBu.GetValue(x.DebitDepartmentCode) : commonBu;
                var dbHojo = dicHojo.GetValue(x.CustomerCode).FirstOrDefault(y => y.KmkId == dbKmk?.Id);
                if (dbKmk == null)
                {
                    valid = false;
                    errorLogger?.Invoke($"借方科目：{x.DebitAccountTitleCode}:{x.DebitAccountTitleName}が存在しません");
                }
                else if (dbKmk.RequireHojo && dbHojo == null)
                {
                    valid = false;
                    errorLogger?.Invoke($"借方補助：{x.DebitAccountTitleCode}:{x.CustomerCode}が存在しません");
                }
                else if (dbBu == null)
                {
                    valid = false;
                    errorLogger?.Invoke($"借方部門：{x.DebitDepartmentCode}:{x.DebitDepartmentName}が存在しません");
                }
                else
                {
                    data.DrBuId = dbBu.Id;
                    data.DrKmkId = dbKmk.Id;
                    data.DrTaxClassId = dbKmk.DrTaxClassId;
                    data.DrHojoId = dbHojo?.Id ?? 0;
                    data.DrMoney = x.Amount;
                }

                var crKmk  = dicKmk.GetValue(x.CreditAccountTitleCode);
                var crBu   = (requireAllDepartment
                           || requirePLDepartment && (crKmk?.IsPLAccount ?? false))
                           ? dicBu.GetValue(x.CreditDepartmentCode) : commonBu;
                var crHojo = dicHojo.GetValue(x.CustomerCode).FirstOrDefault(y => y.KmkId == crKmk?.Id);
                if (crKmk == null)
                {
                    valid = false;
                    errorLogger?.Invoke($"貸方科目：{x.CreditAccountTitleCode}:{x.CreditAccountTitleName}が存在しません");
                }
                else if (crKmk.RequireHojo && crHojo == null)
                {
                    valid = false;
                    errorLogger?.Invoke($"貸方補助：{x.CreditAccountTitleCode}:{x.CurrencyCode}が存在しません");
                }
                else if (crBu == null)
                {
                    valid = false;
                    errorLogger?.Invoke($"貸方部門：{x.CreditDepartmentCode}:{x.CreditDepartmentName}が存在しません");
                }
                else
                {
                    data.CrBuId = crBu.Id;
                    data.CrKmkId = crKmk.Id;
                    data.CrTaxClassId = crKmk.CrTaxClassId;
                    data.CrHojoId = crHojo?.Id ?? 0;
                    data.CrMoney = x.Amount;
                }

                if (!valid) continue;
                list.Add(new BEInputSlip
                {
                    InputSlipHeader = new BEInputSlipHeader {
                        Date = new IntDate(x.RecordedAt),
                    },
                    InputSlipDataList = new InputSlipDataList {
                        BEInputSlipData = new List<BEInputSlipData>(new BEInputSlipData[] { data })
                    }
                });
            }
            return list;
        }


        private static TModel GetValue<TModel>(this Dictionary<string, TModel> dic, string key)
        {
            if (string.IsNullOrEmpty(key) || !dic.ContainsKey(key)) return default(TModel);
            return dic[key];
        }
        private static TModel[] GetValue<TModel>(this SortedList<string, TModel[]> dic, string key)
        {
            if (string.IsNullOrEmpty(key) || !dic.ContainsKey(key)) return new TModel[] { };
            return dic[key];
        }
    }
}
