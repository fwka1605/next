using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class Authentication : IAuthentication
    {
        private IAuthenticationProcessor authenticationProcessor;

        public Authentication(IAuthenticationProcessor authenticationProcessor)
        {
            this.authenticationProcessor = authenticationProcessor;
        }

        public async Task<DataSet> AuthenticateAsync(DataSet arg)
        {
            if (arg == null
                || arg.Tables == null
                || arg.Tables.Count == 0
                || arg.Tables[0].Rows.Count == 0)
            {
                throw new ArgumentException("arg");
            }

            var ds = new DataSet();
            try
            {
                var licenseKey = string.Empty;
                var companyCode = string.Empty;
                var userCode = string.Empty;
                var row = arg
                    ?.Tables.Cast<DataTable>().FirstOrDefault()
                    ?.AsEnumerable().FirstOrDefault();
                licenseKey = row?.Field<string>("AuthenticationKey");
                companyCode = row?.Field<string>("CompanyCode");
                //userCode = row?.Field<string>("UserCode");

                var result = await authenticationProcessor.AuthenticateAsync(licenseKey, companyCode, userCode);

                ds.Tables.Add((new List<AuthenticationResult> { result }).ToDataTable());
            }
            catch (Exception ex)
            {
                ds.Tables.Add(new List<ProcessResult>
                {
                    new ProcessResult
                    {
                        Result = false,
                        ErrorCode = VOne.Common.ErrorCode.ExceptionOccured,
                        ErrorMessage = ex.Message
                    }
                }.ToDataTable());
                //ログ出力
            }
            return ds;
        }

        public Task<DataSet> AuthorizeAsync(DataSet arg)
        {
            throw new NotImplementedException();
        }
    }
}
