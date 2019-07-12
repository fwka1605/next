using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.AccountTransfer.Import.Reader
{
    /// <summary>口座振替結果取込 の 読込・照合処理を行う interface</summary>
    public interface IReader
    {
        /// <summary>テキストファイルの<see cref="Encoding"/> 不適切な場合は削除</summary>
        Encoding Encoding { get; set; }
        /// <summary>非同期処理を行うかどうか</summary>
        bool IsAsync { get; set; }
        /// <summary>直接 テキストファイルを連携するかどうか</summary>
        bool IsPlainText { get; set; }
        /// <summary>会社ID</summary>
        int CompanyId { get; set; }
        /// <summary>決済代行会社ID</summary>
        int PaymentAgencyId { get; set; }
        /// <summary>請求データを 集約するかどうか <see cref="Rac.VOne.Web.Models.Company.TransferAggregate"/>の値が 1 の場合 true</summary>
        bool AggregateBillings { get; set; }
        /// <summary>口座振替の振替年 ※ おおくの口座振替フォーマットで年情報を保持していない</summary>
        int TransferYear { get; set; }
        /// <summary>ファイル名 三菱UFJニコスで、振替日をファイル名から取得する必要がある</summary>
        string FileName { get; set; }
        /// <summary>請求データの取得/得意先マスターの取得などを行うデリゲートを所持</summary>
        Helper Helper { get; set; }
        /// <summary>読込・照合処理</summary>
        /// <param name="file">標準では 取込ファイルの path
        /// 各Reader の <see cref="Rac.VOne.Common.DataHandling.IStreamCreator"/> を変更することで、直接テキストファイルの処理も行える</param>
        /// <returns></returns>
        Task<List<AccountTransferSource>> ReadAsync(string file);
    }

}
