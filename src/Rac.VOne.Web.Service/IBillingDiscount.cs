using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IBillingDiscount" を変更できます。
    [ServiceContract]
    public interface IBillingDiscount
    {
        //[OperationContract]
        //BillingDiscountsResult GetDiscount(string sessionKey, long billingId);

        //[OperationContract]
        //CountResult DeleteDiscount(long billingId, string sessionKey);

        //[OperationContract]
        //CountResult SaveDiscount(Models.BillingDiscount billDiscount, string sessionKey);
    }
}
