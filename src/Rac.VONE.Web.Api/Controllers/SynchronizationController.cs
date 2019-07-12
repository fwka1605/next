using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// 同期確認
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SynchronizationController : ControllerBase
    {
        private readonly ISynchronizationProcessor synchronizationProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public SynchronizationController(
            ISynchronizationProcessor synchronizationProcessor
            )
        {
            this.synchronizationProcessor = synchronizationProcessor;
        }

        //public DataSet CheckCustomer(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckCustomer, logger);
        //}

        //public DataSet CheckCompany(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckCompany, logger);
        //}

        //public DataSet CheckDepartment(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckDepartment, logger);
        //}

        //public DataSet CheckStaff(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckStaff, logger);
        //}

        //public DataSet CheckLoginUser(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckLoginUser, logger);
        //}

        //public DataSet CheckAccountTitle(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckAccountTitle, logger);
        //}

        //public DataSet CheckBankAccount(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckBankAccount, logger);
        //}

        //public DataSet CheckBilling(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckBilling, logger);
        //}

        //public DataSet CheckReceipt(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckReceipt, logger);
        //}

        //public DataSet CheckMatchingHeader(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckMatchingHeader, logger);
        //}

        //public DataSet CheckMatching(DataSet arg)
        //{
        //    return Check(arg, synchronizationProcessor.CheckMatching, logger);
        //}

        //private DataSet Check<TData>(DataSet arg, Func<DateTime, IEnumerable<TData>> check,
        //        ILogger logger = null,
        //        [CallerMemberName]string caller = "")
        //{
        //    if (arg == null
        //        || arg.Tables == null
        //        || arg.Tables.Count == 0
        //        || arg.Tables[0].Rows.Count == 0)
        //    {
        //        throw new ArgumentException("arg");
        //    }

        //    SynchronizationResult<TData> result = null;
        //    var ds = new DataSet();
        //    try
        //    {
        //        var sessionKey = string.Empty;
        //        DateTime updateAt;

        //        var row = arg
        //            ?.Tables.Cast<DataTable>().FirstOrDefault()
        //            ?.AsEnumerable().FirstOrDefault();
        //        sessionKey = row.Field<string>("SessionKey");
        //        updateAt = row.Field<DateTime>("UpdateAt");

        //        result = authorizationProcess.DoAuthorize(sessionKey, () =>
        //        {
        //            TData[] data = check(updateAt).ToArray();
        //            var list = (data == null) ? null : new List<TData>(data);
        //            return new SynchronizationResult<TData>
        //            {
        //                Synchronizations = list,
        //                ProcessResult = new ProcessResult { Result = true },
        //            };
        //        }, logger, caller);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new SynchronizationResult<TData>
        //        {
        //            ProcessResult = new ProcessResult
        //            {
        //                ErrorCode = Rac.VOne.Common.ErrorCode.ExceptionOccured,
        //                ErrorMessage = ex.Message
        //            }
        //        };
        //        //ログ出力
        //    }

        //    ds.Tables.Add((new List<ProcessResult> { result.ProcessResult }).ToDataTable());
        //    //ds.Tables.Add(result.Synchronizations).ToDataTable());
        //    if (result.Synchronizations != null)
        //    {
        //        ds.Tables.Add(result.Synchronizations.ToDataTable("Synchronization"));
        //    }

        //    return ds;
        //}
    }
}
