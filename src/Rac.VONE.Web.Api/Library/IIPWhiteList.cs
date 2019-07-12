using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace Rac.VOne.Web.Api.Library
{
    /// <summary>IP ホワイトリストを取得する interface 定義</summary>
    public interface IIPWhiteList
    {
        /// <summary>接続許可する IPアドレスを IEnumerable で取得</summary>
        /// <remarks>範囲指定を簡単に行いたい場合、nuget で ipaddressrange を利用すれば良い
        /// https://github.com/jsakamoto/ipaddressrange
        /// </remarks>
        IEnumerable<IPAddress> GetIPAddresses();
    }
}
