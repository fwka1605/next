using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Rac.VOne.Client.Common.HttpClients
{
    /// <summary>COM Component を利用して InternetExplorer を起動
    /// 参照オブジェクトに対して <see cref="Marshal.ReleaseComObject(object)"/>の実施が必要
    /// <see cref="System.Windows.Forms.WebBrowser"/>コントロールを利用しようとしたが、
    /// IE7ベースとなっており、最新のIEを利用する場合、クライアントを実行するPCのレジストリの書き換えが必要とのこと
    /// そのため、COM Component を利用する形になった
    /// </summary>
    public class IEWrapper
    {
        public async Task<string> GetAuthorizationCodeAsync(string uri)
        {
            var authorizationCode = string.Empty;
            SHDocVw.InternetExplorer ie = null;
            try
            {
                ie = new SHDocVw.InternetExplorer();
                ie.Navigate(uri);
                await WaitAsync(ie);
                ie.Visible = true;
                do
                {
                    var url = ie.LocationURL;
                    if (url.Contains("?code="))
                    {
                        var index = url.IndexOf("?code=");
                        if (index > 0)
                            authorizationCode = url.Substring(index + 6);
                        break;
                    }
                    await Task.Delay(TimeSpan.FromMilliseconds(100));

                } while (true);
            }
            catch (Exception)
            {
                // ie を閉じたときに ie.LocationURL にアクセスするタイミングで COMException が発生する
            }
            finally
            {
                if (ie != null)
                {
                    try
                    {
                        ie.Quit();
                    }
                    catch (Exception) { }
                    Marshal.ReleaseComObject(ie);
                    ie = null;
                }
            }
            return authorizationCode;
        }

        private async Task WaitAsync(SHDocVw.InternetExplorer ie, int milliseconds = 0)
        {
            while (ie.Busy
                || ie.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
            await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
        }
    }
}
