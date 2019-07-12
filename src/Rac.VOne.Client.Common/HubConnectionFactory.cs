using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNet.SignalR.Client;

namespace Rac.VOne.Client.Common
{
    public class HubConnectionFactory
    {
        private string ApplicationPath { get; set; }

        public HubConnectionFactory()
        {
            var xml = new XmlDocument();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            xml.Load(Path.Combine(path, "VOneServiceSettings.xml"));
            // TODO : 設定ファイル読み込み時のエラー処理

            XmlNode applicationPathNode = xml.GetElementsByTagName("ApplicationPath")
                    .Cast<XmlNode>().FirstOrDefault();
            ApplicationPath = applicationPathNode?.InnerText;
        }

        /// <summary>
        /// MatchingHubConnection作成
        /// </summary>
        /// <param name="onNext">処理更新時に実行されるAction</param>
        /// <param name="action">Webサービス呼出処理</param>
        /// <returns></returns>
        public static HubConnection CreateProgressHubConnection(Action onNext, Action onError, Action<HubConnection, IHubProxy> proxySetter = null)
        {
            var factory = new HubConnectionFactory();
            var hubConnection = factory.CreateConnection();
            var hubProxy = hubConnection.CreateHubProxy("ProgressHub");

            hubProxy.On("UpdateState", onNext);
            hubProxy.On("Abort", onError);

            proxySetter?.Invoke(hubConnection, hubProxy);

            return hubConnection;
        }

        private HubConnection CreateConnection()
        {
            var hubConnection = new HubConnection(Path.Combine(ApplicationPath, "signalr"));
            return hubConnection;
        }
    }
}
