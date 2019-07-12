using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ISynchronization" を変更できます。
    [ServiceContract]
    public interface ISynchronization
    {
        [OperationContract]
        Task<DataSet> CheckCustomerAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> CheckCompanyAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> CheckDepartmentAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> CheckStaffAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> CheckLoginUserAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> CheckAccountTitleAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> CheckBankAccountAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> CheckBillingAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> CheckReceiptAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> CheckMatchingHeaderAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> CheckMatchingAsync(DataSet arg);

    }
}
