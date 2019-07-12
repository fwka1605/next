using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Screen.Importer
{
    public interface IImporter<TModel> where TModel : class, new()
    {
        /// <summary>
        /// インポートファイル読込
        /// </summary>
        /// <returns></returns>
        Task<bool> ReadCsvAsync();
        /// <summary>
        /// インポート処理
        /// </summary>
        /// <returns></returns>
        Task<bool> ImportAsync();
        /// <summary>
        /// インポート後ファイル移動処理
        /// </summary>
        /// <returns></returns>
        bool MoveFile();
        /// <summary>
        /// インポート時検証エラーログの出力処理
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool WriteErrorLog(string path);
        /// <summary>
        /// 帳票印刷用のデータ取得処理
        /// </summary>
        /// <param name="isPossible"></param>
        /// <returns></returns>
        List<TModel> GetReportSource(bool isPossible);
    }
}
