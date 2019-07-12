using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Rac.VOne.Client.Common
{
    public class ServiceProxyFactory
    {
        private static string ApplicationPath { get; set; }
        private static long? MaxReceivedMessageSize { get; set; }
        private static int? MaxBufferSize { get; set; }
        private static long? MaxBufferPoolSize { get; set; }
        private static int? MaxArrayLength { get; set; }

        private static long? OpenCloseTimeout { get; set; }
        private static long? SendReceiveTimeout { get; set; }
        private static TransferMode Mode { get; set; } = TransferMode.Buffered;

        static ServiceProxyFactory()
        {
            var xml = new XmlDocument();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            xml.Load(Path.Combine(path, "VOneServiceSettings.xml"));
            // TODO : 設定ファイル読み込み時のエラー処理

            XmlNode applicationPathNode = xml.GetElementsByTagName("ApplicationPath")
                    .Cast<XmlNode>().FirstOrDefault();
            ApplicationPath = applicationPathNode?.InnerText;

            XmlNode receivedSizeNode = xml.GetElementsByTagName("MaxReceivedMessageSize")
                    .Cast<XmlNode>().FirstOrDefault();
            long receivedSize = 0;
            if (long.TryParse(receivedSizeNode?.InnerText ?? "0", out receivedSize))
            {
                MaxReceivedMessageSize = receivedSize;
            }

            XmlNode bufferSizeNode = xml.GetElementsByTagName("MaxBufferSize")
                    .Cast<XmlNode>().FirstOrDefault();
            int bufferSize = 0;
            if (int.TryParse(bufferSizeNode.InnerText ?? "0", out bufferSize))
            {
                MaxBufferSize = bufferSize;
            }

            XmlNode bufferPoolSizeNode = xml.GetElementsByTagName("MaxBufferPoolSize")
                    .Cast<XmlNode>().FirstOrDefault();
            long bufferPoolSize = 0;
            if (long.TryParse(bufferPoolSizeNode.InnerText, out bufferPoolSize))
            {
                MaxBufferPoolSize = bufferPoolSize;
            }

            XmlNode arrayLengthNode = xml.GetElementsByTagName("MaxArrayLength")
                    .Cast<XmlNode>().FirstOrDefault();
            int arrayLength;
            if (int.TryParse(arrayLengthNode.InnerText, out arrayLength))
            {
                MaxArrayLength = arrayLength;
            }

            XmlNode openCloseTimeoutNode = xml.GetElementsByTagName("OpenCloseTimeoutSecond")
                    .Cast<XmlNode>().FirstOrDefault();
            long openCloseTimeout = 0;
            if (long.TryParse(openCloseTimeoutNode.InnerText, out openCloseTimeout))
            {
                OpenCloseTimeout = openCloseTimeout;
            }

            XmlNode sendReceiveTimeoutNode = xml.GetElementsByTagName("SendReceiveTimeoutSecond")
                    .Cast<XmlNode>().FirstOrDefault();
            long sendReceiveTimeout = 0;
            if (long.TryParse(sendReceiveTimeoutNode.InnerText, out sendReceiveTimeout))
            {
                SendReceiveTimeout = sendReceiveTimeout;
            }

            XmlNode transferModeNode = xml.GetElementsByTagName("TransferMode")
                    .Cast<XmlNode>().FirstOrDefault();
            TransferMode mode;
            if (Enum.TryParse(transferModeNode.InnerText, out mode))
            {
                Mode = mode;
            }
        }

        public static TService CreateWcfService<TService>()
            where TService : class
        {
            const string suffix = "Client";
            Type serviceType = typeof(TService);
            string serviceName = null;
            string subDirectory = string.Empty;
            if (serviceType.IsInterface)
            {
                serviceName = serviceType.Name.Substring(1);
                serviceType = serviceType.Assembly.GetTypes()
                        .Where(t => t.Name == serviceName + suffix
                            && t.Namespace == serviceType.Namespace)
                        .FirstOrDefault();
            }
            else
            {
                if (serviceType.Name.EndsWith(suffix))
                {
                    serviceName = serviceType.Name
                            .Substring(0, serviceType.Name.Length - suffix.Length);
                }
                else
                {
                    serviceType = null;
                }
            }

            if (serviceType == null)
            {
                throw new ArgumentException(
                        $"{serviceName}：サービスが見つかりませんでした。",
                        nameof(TService));
            }

            if (serviceType.GetCustomAttribute<MasterServiceAttribute>() != null)
            {
                subDirectory = "Master";
            }

            var binding = new BasicHttpBinding();
            if (MaxBufferPoolSize.HasValue)
            {
                binding.MaxBufferPoolSize = MaxBufferPoolSize.Value;
            }
            if (MaxBufferSize.HasValue)
            {
                binding.MaxBufferSize = MaxBufferSize.Value;
            }
            if (MaxReceivedMessageSize.HasValue)
            {
                binding.MaxReceivedMessageSize = MaxReceivedMessageSize.Value;
            }
            if (MaxArrayLength.HasValue)
            {
                binding.ReaderQuotas.MaxArrayLength = MaxArrayLength.Value;
            }

            if (OpenCloseTimeout.HasValue)
            {
                binding.OpenTimeout = TimeSpan.FromSeconds(OpenCloseTimeout.Value);
                binding.CloseTimeout = binding.OpenTimeout;
            }
            if (SendReceiveTimeout.HasValue)
            {
                binding.SendTimeout = TimeSpan.FromSeconds(SendReceiveTimeout.Value);
                binding.ReceiveTimeout = binding.SendTimeout;
            }
            binding.TransferMode = Mode;

            string endpoint = Path.Combine(ApplicationPath, subDirectory, serviceName + ".svc");

            return Activator.CreateInstance(serviceType,
                    binding,
                    new EndpointAddress(endpoint)) as TService;
        }

        /// <summary>
        /// Web Service Client を複数作成し、処理する場合に利用
        /// 例外が発生した場合や、処理が完了したタイミングで、クライアントを 確実に Close するための処理
        /// 単独での利用は Do / DoAsync の利用を推奨
        /// </summary>
        /// <param name="effectiveBlock"></param>
        public static void LifeTime(Action<ServiceProxyFactory> effectiveBlock)
        {
            var factory = new ServiceProxyFactory();
            try
            {
                effectiveBlock(factory);
                factory.Close(); // ここからExceptionは発生しない
            }
            catch (Exception ex)
                when (ex is CommunicationObjectFaultedException
                    || ex is TimeoutException)
            {
                factory.Abort();
            }
            catch (Exception)
            {
                factory.Abort();
                throw;
            }
        }

        /// <summary>
        /// Web Service Client を複数作成し、処理する場合に利用
        /// 例外が発生した場合や、処理が完了したタイミングで、クライアントを 確実に Close するための処理
        /// 単独での利用は Do / DoAsync の利用を推奨
        /// </summary>
        /// <param name="effectiveBlock"></param>
        /// <returns></returns>
        public static async Task LifeTime(Func<ServiceProxyFactory, Task> effectiveBlock)
        {
            var factory = new ServiceProxyFactory();
            try
            {
                await effectiveBlock(factory);
                factory.Close(); // ここからExceptionは発生しない
            }
            //catch (Exception ex)
            //    when (ex is CommunicationObjectFaultedException
            //        || ex is TimeoutException)
            //{
            //    factory.Abort();
            //}
            catch (Exception)
            {
                factory.Abort();
                throw;
            }
        }

       /// <summary>
       /// WCF ServiceClient を指定して処理実行 戻り値なし 同期処理
       /// 一回の処理で 複数の Clientを Create しない場合はこちらを利用する
       /// </summary>
       /// <typeparam name="TClient"></typeparam>
       /// <param name="action"></param>
        public static void Do<TClient>(Action<TClient> action)
            where TClient : class, ICommunicationObject
        {
            LifeTime(factory =>
            {
                var client = factory.Create<TClient>();
                action(client);
            });
        }

        /// <summary>
        /// WCF ServiceClient を指定して処理実行 戻り値あり<see cref="TResult"/> 同期処理
        /// 一回の処理で 複数の Client を Create しない場合はこちらを利用する
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TResult Do<TClient, TResult>(Func<TClient, TResult> action)
            where TClient : class, ICommunicationObject
        {
            TResult result = default(TResult);
            LifeTime(factory =>
            {
                var client = factory.Create<TClient>();
                result = action(client);
            });
            return result;
        }

        /// <summary>
        /// WCF ServiceClient を指定して処理実行 戻り値なし 非同期
        /// 一回の処理で 複数の Client を Create しない場合はこちらを利用する
        /// </summary>
        /// <typeparam name="TClient">WCF ServiceClient</typeparam>
        /// <param name="action">clientのメソッドのコールなど</param>
        /// <returns></returns>
        public static async Task DoAsync<TClient>(Func<TClient, Task> action)
            where TClient : class, ICommunicationObject
        {
            await LifeTime(async factory =>
            {
                var client = factory.Create<TClient>();
                await action(client);
            });
        }

        /// <summary>
        /// WCF ServiceClient を指定して処理実行 戻り値<see cref="TResult"/> 非同期
        /// 一回の処理で 複数の Client を Create しない場合はこちらを利用する
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task<TResult> DoAsync<TClient, TResult>(Func<TClient, Task<TResult>> action)
            where TClient : class, ICommunicationObject
        {
            TResult result = default(TResult);
            await LifeTime(async factory =>
            {
                var client = factory.Create<TClient>();
                result = await action(client);
            });
            return result;
        }

        #region インスタンスメンバー
        private Dictionary<Type, ICommunicationObject> WcfClients { get; }
            = new Dictionary<Type, ICommunicationObject>();

        internal ServiceProxyFactory()
        {
        }

        public TClient Create<TClient>()
            where TClient : class, ICommunicationObject
        {
            ICommunicationObject service = null;
            if (!WcfClients.TryGetValue(typeof(TClient), out service))
            {
                service = CreateWcfService<TClient>();
                WcfClients.Add(typeof(TClient), service);
            }
            return service as TClient;
        }

        internal void Close()
        {
            foreach (ICommunicationObject client in WcfClients.Values)
            {
                try
                {
                    client.Close();
                }
                catch (Exception) // CommunicationObjectFaultedException or TimeoutException
                {
                    client.Abort();
                }
            }
        }

        internal void Abort()
        {
            foreach (ICommunicationObject client in WcfClients.Values)
            {
                client.Abort();
            }
        }
        #endregion
    }
}
