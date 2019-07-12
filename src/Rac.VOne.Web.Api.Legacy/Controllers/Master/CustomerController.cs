using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  得意先マスター
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CustomerController : ApiControllerAuthorized
    {
        private readonly ICustomerProcessor customerProcessor;
        private readonly ICustomerPaymentContractProcessor customerPaymentContractProcessor;
        private readonly ICustomerDiscountProcessor customerDiscountProcessor;

        private readonly ICustomerFileImportProcessor customerImportProcessor;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomerController(
            ICustomerProcessor customerProcessor,
            ICustomerPaymentContractProcessor customerPaymentContractProcessor,
            ICustomerDiscountProcessor customerDiscountProcessor,
            ICustomerFileImportProcessor customerImportProcessor
            )
        {
            this.customerProcessor = customerProcessor;
            this.customerPaymentContractProcessor = customerPaymentContractProcessor;
            this.customerDiscountProcessor = customerDiscountProcessor;
            this.customerImportProcessor = customerImportProcessor;
        }

        /// <summary>
        /// 得意先 配列取得
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Customer>> GetItems(CustomerSearch option, CancellationToken token)
            => (await customerProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 担当者ID<see cref="Staff.Id"/>が登録されているか確認する処理
        /// </summary>
        /// <param name="staffId">担当者ID</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<bool> ExistStaff([FromBody] int staffId, CancellationToken token)
            => await customerProcessor.ExistStaffAsync(staffId, token);

        /// <summary>
        /// 区分ID<see cref="Category.Id"/>が登録されているか確認する処理
        /// </summary>
        /// <param name="categoryId">区分ID</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<bool> ExistCategory([FromBody] int categoryId, CancellationToken token)
            => await customerProcessor.ExistCategoryAsync(categoryId, token);

        /// <summary>
        /// 会社ID<see cref="Company.Id"/>が登録されているか確認する処理
        /// </summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistCompany([FromBody] int companyId, CancellationToken token)
            => await customerProcessor.ExistCompanyAsync(companyId, token);

        /// <summary>登録</summary>
        /// <param name="customer">得意先マスター</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<Customer> Save(Customer customer, CancellationToken token)
            => await customerProcessor.SaveAsync(customer, requireIsParentUpdate: true, token: token);

        /// <summary>登録 複数件</summary>
        /// <param name="customers">得意先 配列</param>
        /// <param name="token">自動バインド</param>
        /// <remarks>IsParent の 更新は 行わない</remarks>
        [HttpPost]
        public async Task<IEnumerable<Customer>> SaveItems(IEnumerable<Customer> customers, CancellationToken token)
            => (await customerProcessor.SaveItemsAsync(customers, token)).ToArray();

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="customerId">得意先ID</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<int> Delete([FromBody] int customerId, CancellationToken token)
            => await customerProcessor.DeleteAsync(customerId, token);

        /// <summary>
        /// 得意先 歩引き情報の登録
        /// </summary>
        /// <param name="customerDiscount">歩引き情報</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<CustomerDiscount> SaveDiscount(CustomerDiscount customerDiscount, CancellationToken token)
            => await customerDiscountProcessor.SaveAsync(customerDiscount, token);

        /// <summary>
        /// 得意先歩引き情報の削除
        /// </summary>
        /// <param name="discount">得意先ID,歩引きID <see cref="CustomerDiscount.Sequence"/>を 指定 </param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> DeleteDiscount(CustomerDiscount discount, CancellationToken token)
            => await customerDiscountProcessor.DeleteAsync(discount, token);

        /// <summary>
        /// 得意先歩引き情報取得
        /// </summary>
        /// <param name="customerId">得意先ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CustomerDiscount>> GetDiscount([FromBody] int customerId, CancellationToken token)
            => (await customerDiscountProcessor.GetAsync(customerId)).ToArray();

        /// <summary>
        /// インポート処理
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await customerImportProcessor.ImportAsync(source, token);


        /// <summary>コード検索ダイアログ用 <see cref="IEnumerable{CustomerMin}"/>の取得</summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns><see cref="IEnumerable{CustomerMin}"/></returns>
        [HttpPost]
        public async Task<IEnumerable<CustomerMin>> GetMinItems([FromBody] int companyId, CancellationToken token)
            => (await customerProcessor.GetMinItemsAsync(companyId, token)).ToArray();


    }
}
