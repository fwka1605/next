using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "BillingDiscount" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで BillingDiscount.svc または BillingDiscount.svc.cs を選択し、デバッグを開始してください。
    ////public class BillingDiscount : IBillingDiscount
    ////{
        //private IAuthorizationProcessor authorizationProcess = null;
        //private IBillingDiscountProcessor discountProcessor = null;

        //public BillingDiscount(IAuthorizationProcessor authorization,
        //   IBillingDiscountProcessor discountProcessor)
        //{
        //    authorizationProcess = authorization;
        //    this.discountProcessor = discountProcessor;
        //}

        //public BillingDiscountsResult GetDiscount(string sessionKey, long billingId)
        //{
        //    return authorizationProcess.DoAuthorize(sessionKey, () =>
        //    {
        //        List<Web.Models.BillingDiscount> result
        //                = discountProcessor.GetDiscount(billingId).ToList();
        //        return new BillingDiscountsResult
        //        {
        //            ProcessResult = new ProcessResult { Result = true },
        //            BillingDiscounts = result,
        //        };
        //    });
        //}

        //public CountResult DeleteDiscount(long billingId, string sessionKey)
        //{
        //    return authorizationProcess.DoAuthorize(sessionKey, scopeBuilder =>
        //    {
        //        int result = 0;
        //        using (var scope = scopeBuilder.Create())
        //        {
        //            result =  discountProcessor.DeleteDiscount(billingId);
        //            scope.Complete();
        //        }
        //        return new CountResult
        //        {
        //            ProcessResult = new ProcessResult { Result = true },
        //            Count = result,
        //        };
        //    });
        //}

        //public CountResult SaveDiscount(Models.BillingDiscount billDiscount, string sessionKey)
        //{
        //    return authorizationProcess.DoAuthorize(sessionKey, scopeBuilder =>
        //    {
        //        int result = 0;
        //        using (var scope = scopeBuilder.Create())
        //        {
        //            result = discountProcessor.SaveDiscount(billDiscount);
        //            scope.Complete();
        //        }
        //        return new CountResult
        //        {
        //            ProcessResult = new ProcessResult { Result = true },
        //            Count = result,
        //        };
        //    });
        //}
   // }
}
