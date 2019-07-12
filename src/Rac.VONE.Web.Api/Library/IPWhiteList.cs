using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

namespace Rac.VOne.Web.Api.Library
{
    /// <summary>IP制限 ホワイトリスト取得用</summary>
    public class IPWhiteList : IIPWhiteList
    {
        private readonly IConfiguration configuration;

        /// <summary></summary>
        public IPWhiteList(IConfiguration configuration ) {
            this.configuration = configuration;
        }

        /// <summary>利用可能な <see cref="IPAddress"/>の IEnumerable 取得</summary>
        public IEnumerable<IPAddress> GetIPAddresses()
        {
            var value = configuration["IPWhiteList"];
            // range 指定したい場合は library 入れる
            foreach (var item in value.Split(";"))
                if (IPAddress.TryParse(item, out var address)) yield return address;
        }
    }
}
