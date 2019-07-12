using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class PaymentAgency : IMaster
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Kana { get; set; }
        [DataMember] public string ConsigneeCode { get; set; }
        [DataMember] public int ShareTransferFee { get; set; }
        [DataMember] public int UseFeeLearning { get; set; }
        [DataMember] public int UseFeeTolerance { get; set; }
        [DataMember] public int DueDateOffset { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public int AccountTypeId { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public int FileFormatId { get; set; }
        [DataMember] public int ConsiderUncollected { get; set; }
        [DataMember] public int? CollectCategoryId { get; set; }
        [DataMember] public int UseKanaLearning { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string OutputFileName { get; set; }
        [DataMember] public int AppendDate { get; set; }
        [DataMember] public string ContractCode { get; set; }

        /// <summary>
        ///  決済代行会社 マスターに設定されているファイル名を取得
        /// </summary>
        /// <param name="reoutput">
        /// 再出力の場合 固定で Re_ という文字が追加される
        /// </param>
        /// <returns></returns>
        public string GetOutputFileName(bool reoutput = false)
            => $"{(reoutput ? "Re_" : "")}{OutputFileName}{(AppendDate == 1 ? DateTime.Today.ToString("yyyyMMdd") : "")}.csv";
    }

    [DataContract]
    public class PaymentAgencyResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public PaymentAgency PaymentAgency { get; set; }
    }

    [DataContract]
    public class PaymentAgenciesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<PaymentAgency> PaymentAgencies { get; set; }
    }
}
