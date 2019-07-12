using System;
using System.Configuration;
using System.Linq;
using Rac.VOne.Common;
using Rac.VOne.Data;

namespace Rac.VOne.Web.Api
{
    using Microsoft.Extensions.Configuration;

    /// <summary>アプリケーション設定取得用の実装</summary>
    /// <remarks>
    /// <see cref="Microsoft.Extensions.Configuration.IConfiguration"/>を inject すれば、
    /// どこでも利用可能なので、敢えて interface や その実装を用意する意義自体が薄い
    /// obsolete class にした方が良いかも
    /// </remarks>
    public class ApplicationSettings : ISetting, IConnectionString, ITimeOutSetter
    {
        private readonly IConfiguration configuration;

        /// <summary></summary>
        public ApplicationSettings(
            IConfiguration configuration
            )
        {
            this.configuration = configuration;
        }


        /// <summary></summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSetting(string key) => configuration[key];


        /// <summary>接続文字列の取得 appsetting.json の 既定の接続文字列設定に沿った設定をしている場合に取得可能</summary>
        /// <param name="name">接続文字列の名称</param>
        /// <returns>接続文字列
        /// </returns>
        /// <remarks>
        /// 接続先が 名称によって変更されうるタイプのものではないので、 引数の name は不要
        /// 結局 呼出し側で 固定値使うことになるのであれば、実装側で固定値を使用し、
        /// 引数を空にした方が、何を呼び出しているか飛んで確認しなくてよくなる
        /// </remarks>
        public string GetConnectionString(string name) => configuration.GetConnectionString(name);

        /// <summary>
        /// タイムアウトの取得
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeOut()
        {
            var value = GetSetting("TransactionTimeoutSecond");
            var second = 0;
            if (int.TryParse(value, out second)) return TimeSpan.FromSeconds(second);
            throw new ArgumentException("TransactionTimeoutSecound is not setted.");
        }

        /// <summary>接続文字列</summary>
        public string ConnectionString => configuration.GetConnectionString("DefaultConnection");

    }
}
