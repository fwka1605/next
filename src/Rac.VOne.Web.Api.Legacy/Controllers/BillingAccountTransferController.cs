﻿using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Common.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>請求・口座振替用</summary>
    public class BillingAccountTransferController : ApiControllerAuthorized
    {
        private readonly IBillingAccountTransferFileImportProcessor billingAccountTransferFileImportProcessor;

        /// <summary>constructor</summary>
        public BillingAccountTransferController(
            IBillingAccountTransferFileImportProcessor billingAccountTransferFileImportProcessor
            )
        {
            this.billingAccountTransferFileImportProcessor = billingAccountTransferFileImportProcessor;
        }

        /// <summary>口座振替結果取込 読込・検証</summary>
        /// <param name="source">
        /// <see cref="AccountTransferImportSource.CompanyId"/>,
        /// <see cref="AccountTransferImportSource.PaymentAgencyId"/>,
        /// <see cref="AccountTransferImportSource.EncodingCodePage"/>,
        /// <see cref="AccountTransferImportSource.Data"/>,
        /// <see cref="AccountTransferImportSource.FileName"/>,
        /// <see cref="AccountTransferImportSource.TransferYear"/>,
        /// <see cref="AccountTransferImportSource.LoginUserId"/>, を指定
        /// </param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AccountTransferImportResult> Read(AccountTransferImportSource source, CancellationToken token)
            => await billingAccountTransferFileImportProcessor.ReadAsync(source, token);

        /// <summary>口座振替結果取込 登録</summary>
        /// <param name="source">
        /// <see cref="AccountTransferImportSource.ImportDataId"/>,
        /// <see cref="AccountTransferImportSource.PaymentAgencyId"/>,
        /// <see cref="AccountTransferImportSource.LoginUserId"/>,
        /// <see cref="AccountTransferImportSource.NewCollectCategoryId"/>を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AccountTransferImportResult> Import(AccountTransferImportSource source, CancellationToken token)
            => await billingAccountTransferFileImportProcessor.ImportAsync(source, token);

    }
}