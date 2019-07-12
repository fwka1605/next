using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Batch
{
    /// <summary>
    /// 各種バッチ処理の メイン処理呼出し用
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// バッチ処理 メイン処理の実施
        /// </summary>
        /// <returns></returns>
        bool Work();

        /// <summary>
        /// ファイル移動
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        bool MoveTo(bool success);

        /// <summary>
        /// バッチ処理ログの登録
        /// </summary>
        /// <param name="success">成否</param>
        /// <param name="startAt">開始日時</param>
        /// <param name="endAt">終了日時</param>
        /// <param name="errorLogPath">エラーログの保存先 絶対パス</param>
        /// <returns></returns>
        bool RegisterLog(bool success, DateTime startAt, DateTime endAt, string errorLogPath);
    }
}
