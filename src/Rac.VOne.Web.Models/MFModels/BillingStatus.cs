using System;
using System.Collections.Generic;
using System.Text;

namespace Rac.VOne.Web.Models.MFModels
{
    /// <summary>請求状態</summary>
    public class billing_status
    {
        /// <summary>入金ステータス
        /// 0 : 未設定
        /// 1 : 未入金
        /// 2 : 入金済み </summary>
        /// <remarks>
        /// 請求データ 消込完了時に 2: 入金済
        /// 消込解除を行ったタイミングで 0 : 未設定 とする（すべて解除などは考慮しない）
        /// </remarks>
        public string payment { get; set; }
    }
}
