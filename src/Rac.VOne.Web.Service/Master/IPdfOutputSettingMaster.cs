using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IPdfOutputSettingMaster" を変更できます。
    [ServiceContract]
    public interface IPdfOutputSettingMaster
    {
        [OperationContract]
        Task<PdfOutputSettingResult> GetAsync(string SessionKey, int CompanyId, int ReportType, int UserId);
        [OperationContract]
        Task<PdfOutputSettingResult> SaveAsync(string SessionKey, PdfOutputSetting Setting);
    }
}
