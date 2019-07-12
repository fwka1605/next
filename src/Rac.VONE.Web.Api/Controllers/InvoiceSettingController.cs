using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  請求書設定関連 設定/請求書番号履歴/テンプレート 登録/削除/取得
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvoiceSettingController : ControllerBase
    {
        private readonly IInvoiceCommonSettingProcessor invoiceCommonSettingProcessor;
        private readonly IInvoiceNumberHistoryProcessor invoiceNumberHistoryProcessor;
        private readonly IInvoiceNumberSettingProcessor invoiceNumberSettingProcessor;
        private readonly IInvoiceTemplateSettingProcessor invoiceTemplateSettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public InvoiceSettingController(
            IInvoiceCommonSettingProcessor invoiceCommonSettingProcessor,
            IInvoiceNumberHistoryProcessor invoiceNumberHistoryProcessor,
            IInvoiceNumberSettingProcessor invoiceNumberSettingProcessor,
            IInvoiceTemplateSettingProcessor invoiceTemplateSettingProcessor
            )
        {
            this.invoiceCommonSettingProcessor = invoiceCommonSettingProcessor;
            this.invoiceNumberHistoryProcessor = invoiceNumberHistoryProcessor;
            this.invoiceNumberSettingProcessor = invoiceNumberSettingProcessor;
            this.invoiceTemplateSettingProcessor = invoiceTemplateSettingProcessor;
        }

        /// <summary>
        /// 請求書設定 取得
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< InvoiceCommonSetting >> GetInvoiceCommonSetting([FromBody] int companyId, CancellationToken token)
            => await invoiceCommonSettingProcessor.GetAsync(companyId, token);

        /// <summary>
        /// 請求書設定 登録
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< InvoiceCommonSetting >> SaveInvoiceCommonSetting(InvoiceCommonSetting setting, CancellationToken token)
            => await invoiceCommonSettingProcessor.SaveAsync(setting);


        /// <summary>
        /// 請求書番号 履歴？？？ 配列取得
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<InvoiceNumberHistory>>> GetInvoiceNumberHistories([FromBody] int companyId, CancellationToken token)
            => (await invoiceNumberHistoryProcessor.GetItemsAsync(companyId, token)).ToArray();

        /// <summary>
        /// 請求書番号 履歴？？？ 登録
        /// </summary>
        /// <param name="history"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<InvoiceNumberHistory>> SaveInvoiceNumberHistory(InvoiceNumberHistory history, CancellationToken token)
            => await invoiceNumberHistoryProcessor.SaveAsync(history, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> DeleteInvoiceNumberHistories([FromBody] int companyId, CancellationToken token)
            => await invoiceNumberHistoryProcessor.DeleteAsync(companyId, token);

        /// <summary>
        /// 請求書番号 設定 取得
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< InvoiceNumberSetting >> GetInvoiceNumberSetting([FromBody] int companyId, CancellationToken token)
            => await invoiceNumberSettingProcessor.GetAsync(companyId, token);

        /// <summary>
        /// 請求書番号 設定 登録
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< InvoiceNumberSetting >> SaveInvoiceNumberSetting(InvoiceNumberSetting setting, CancellationToken token)
            => await invoiceNumberSettingProcessor.SaveAsync(setting, token);

        /// <summary>
        /// 回収区分ID の登録確認
        /// </summary>
        /// <param name="collectCategoryId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistCollectCategoryAtTemplate([FromBody] int collectCategoryId, CancellationToken token)
            => await invoiceTemplateSettingProcessor.ExistCollectCategoryAsync(collectCategoryId, token);

        /// <summary>
        /// 請求書テンプレートの取得
        /// </summary>
        /// <param name="setting">会社ID,コードを指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< InvoiceTemplateSetting >> GetInvoiceTemplateSettingByCode(InvoiceTemplateSetting setting, CancellationToken token)
            => await invoiceTemplateSettingProcessor.GetByCodeAsync(setting.CompanyId, setting.Code, token);

        /// <summary>
        /// 請求書テンプレート 配列取得
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< InvoiceTemplateSetting >>> GetInvoiceTemplateSettings([FromBody] int companyId, CancellationToken token)
            => (await invoiceTemplateSettingProcessor.GetItemsAsync(companyId, token)).ToArray();

        /// <summary>
        /// 請求書テンプレート 登録
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< InvoiceTemplateSetting >> SaveInvoiceTemplateSetting(InvoiceTemplateSetting setting, CancellationToken token)
            => await invoiceTemplateSettingProcessor.SaveAsync(setting, token);

        /// <summary>
        /// 請求書テンプレート 削除
        /// </summary>
        /// <param name="id">テンプレートID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> DeleteInvoiceTemplateSetting([FromBody] int id, CancellationToken token)
            => await invoiceTemplateSettingProcessor.DeleteAsync(id, token);

    }
}
