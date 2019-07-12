using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.Security;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Web.Common
{
    public class CompanyInitializeProcessor : ICompanyInitializeProcessor
    {
        private readonly IAddCompanyQueryProcessor              addCompanyQueryProcessor;

        private readonly IAddCompanyLogoQueryProcessor          addCompanyLogoQueryProcessor;
        private readonly IDeleteCompanyLogoQueryProcessor       deleteCompanyLogoQueryProcessor;

        private readonly IAddDepartmentQueryProcessor           addDepartmentQueryProcessor;
        private readonly IAddLoginUserQueryProcessor            addLoginUserQueryProcessor;
        private readonly IAddLoginUserPasswordQueryProcessor    addLoginUserPasswordQueryProcessor;
        private readonly ICategoriesQueryProcessor              categoryQueryProcessor;
        private readonly IAddCollationSettingQueryProcessor     collationSettingQueryProcessor;
        private readonly IAddColumnNameSettingQueryProcessor    columnNameSettingQueryProcessor;
        private readonly IAddCurrencyQueryProcessor             currencyQueryProcessor;
        private readonly IGeneralSettingQueryProcessor          generalSettingQueryProcessor;
        private readonly IJuridicalPersonalityQueryProcessor    juridicalPersonalityQueryProcessor;
        private readonly IInitializeImportSettingQueryProcessor importSettingQueryProcessor;
        private readonly IAddStatusQueryProcessor               addStatusQueryProcessor;
        private readonly IInvoiceCommonSettingProcessor         invoiceCommonSettingProcessor;
        private readonly IInvoiceNumberSettingProcessor         invoiceNumberSettingProcessor;
        private readonly IInvoiceTemplateSettingProcessor       invoiceTemplateSettingProcessor;
        private readonly IAddApplicationControlQueryProcessor   addApplicationControlQueryProcessor;
        private readonly IAddMenuAuthorityQueryProcessor        addMenuAuthorityQueryProcessor;
        private readonly IAddFunctionAuthorityQueryProcessor    addFunctionAuthorityQueryProcessor;
        private readonly IAddPasswordPolicyQueryProcessor       addPasswordPolicyQueryProcessor;
        private readonly ILoginUserLicenseQueryProcessor        loginUserLicenseQueryProcessor;
        private readonly ICollationOrderQueryProcessor          collationOrderQueryProcessor;
        private readonly IMatchingOrderQueryProcessor           matchingOrderQueryProcessor;

        private readonly IHashAlgorithm             hashAlgorithm;
        private readonly ITransactionScopeBuilder   transactionScopeBuilder;

        public CompanyInitializeProcessor(
            IAddCompanyQueryProcessor addCompanyQueryProcessor,

            IAddCompanyLogoQueryProcessor           addCompanyLogoQueryProcessor,
            IDeleteCompanyLogoQueryProcessor        deleteCompanyLogoQueryProcessor,

            IAddDepartmentQueryProcessor            addDepartmentQueryProcessor,
            IAddLoginUserQueryProcessor             addLoginUserQueryProcessor,
            IAddLoginUserPasswordQueryProcessor     addLoginUserPasswordQueryProcessor,
            ICategoriesQueryProcessor               categoryQueryProcessor,
            IAddCollationSettingQueryProcessor      collationSettingQueryProcessor,
            IAddColumnNameSettingQueryProcessor     columnNameSettingQueryProcessor,
            IAddCurrencyQueryProcessor              currencyQueryProcessor,
            IGeneralSettingQueryProcessor           generalSettingQueryProcessor,
            IJuridicalPersonalityQueryProcessor     juridicalPersonalityQueryProcessor,
            IInitializeImportSettingQueryProcessor  importSettingQueryProcessor,
            IAddStatusQueryProcessor                addStatusQueryProcessor,
            IInvoiceCommonSettingProcessor          invoiceCommonSettingProcessor,
            IInvoiceNumberSettingProcessor          invoiceNumberSettingProcessor,
            IInvoiceTemplateSettingProcessor        invoiceTemplateSettingProcessor,

            IAddApplicationControlQueryProcessor    addApplicationControlQueryProcessor,
            IAddMenuAuthorityQueryProcessor         addMenuAuthorityQueryProcessor,
            IAddFunctionAuthorityQueryProcessor     addFunctionAuthorityQueryProcessor,
            IAddPasswordPolicyQueryProcessor        addPasswordPolicyQueryProcessor,
            ILoginUserLicenseQueryProcessor         loginUserLicenseQueryProcessor,
            ICollationOrderQueryProcessor           collationOrderQueryProcessor,
            IMatchingOrderQueryProcessor            matchingOrderQueryProcessor,

            IHashAlgorithm                          hashAlgorithm,
            ITransactionScopeBuilder                transactionScopeBuilder
            )
        {
            this.addCompanyQueryProcessor               = addCompanyQueryProcessor;

            this.addCompanyLogoQueryProcessor           = addCompanyLogoQueryProcessor;
            this.deleteCompanyLogoQueryProcessor        = deleteCompanyLogoQueryProcessor;

            this.addDepartmentQueryProcessor            = addDepartmentQueryProcessor;
            this.addLoginUserQueryProcessor             = addLoginUserQueryProcessor;
            this.addLoginUserPasswordQueryProcessor     = addLoginUserPasswordQueryProcessor;
            this.categoryQueryProcessor                 = categoryQueryProcessor;
            this.collationSettingQueryProcessor         = collationSettingQueryProcessor;
            this.columnNameSettingQueryProcessor        = columnNameSettingQueryProcessor;
            this.currencyQueryProcessor                 = currencyQueryProcessor;
            this.generalSettingQueryProcessor           = generalSettingQueryProcessor;
            this.juridicalPersonalityQueryProcessor     = juridicalPersonalityQueryProcessor;
            this.importSettingQueryProcessor            = importSettingQueryProcessor;
            this.addStatusQueryProcessor                = addStatusQueryProcessor;
            this.invoiceCommonSettingProcessor          = invoiceCommonSettingProcessor;
            this.invoiceNumberSettingProcessor          = invoiceNumberSettingProcessor;
            this.invoiceTemplateSettingProcessor        = invoiceTemplateSettingProcessor;

            this.addApplicationControlQueryProcessor    = addApplicationControlQueryProcessor;
            this.addMenuAuthorityQueryProcessor         = addMenuAuthorityQueryProcessor;
            this.addFunctionAuthorityQueryProcessor     = addFunctionAuthorityQueryProcessor;
            this.addPasswordPolicyQueryProcessor        = addPasswordPolicyQueryProcessor;
            this.loginUserLicenseQueryProcessor         = loginUserLicenseQueryProcessor;
            this.collationOrderQueryProcessor           = collationOrderQueryProcessor;
            this.matchingOrderQueryProcessor            = matchingOrderQueryProcessor;

            this.hashAlgorithm                          = hashAlgorithm;
            this.transactionScopeBuilder                = transactionScopeBuilder;
        }

        private string GetInitialCode(int length)
        {
            return ("1").PadLeft(length, '0');
        }

        private async Task<Department> InitializeDepartmentAsync(CompanySource source, CancellationToken token)
        {
            var department = new Department
            {
                CompanyId = source.Company.Id,
                Code = GetInitialCode(source.ApplicationControl.DepartmentCodeLength),
                Name = source.Company.Name,
                Note = "",
                CreateBy = 0,
                UpdateBy = 0,
            };
            return await addDepartmentQueryProcessor.SaveAsync(department, token);
        }

        private async Task<LoginUser> InitializeLoginUserAsync(CompanySource source, Department department, CancellationToken token)
        {
            var loginUser = new LoginUser
            {
                CompanyId = source.Company.Id,
                Code = GetInitialCode(source.ApplicationControl.LoginUserCodeLength),
                Name = "システム管理者",
                DepartmentId = department.Id,
                Mail = "",
                MenuLevel = 1,
                FunctionLevel = 1,
                UseClient = 1,
                UseWebViewer = 0,
                StringValue1 = "",
                StringValue2 = "",
                StringValue3 = "",
                StringValue4 = "",
                StringValue5 = "",
                CreateBy = 0,
                UpdateBy = 0,
            };
            return await addLoginUserQueryProcessor.SaveAsync(loginUser, token);
        }

        private async Task InitializeLoginUserPasswordAsync(CompanySource source, LoginUser loginUser, CancellationToken token)
        {
            var hash = hashAlgorithm.Compute(source.PasswordPolicy.Convert("password"));
            var password = new LoginUserPassword
            {
                LoginUserId = loginUser.Id,
                PasswordHash = hash,
            };
            await addLoginUserPasswordQueryProcessor.SaveAsync(password);
        }

        private async Task InitializeCollationSettingAsync(CompanySource source, CancellationToken token)
        {
            var setting = new CollationSetting
            {
                CompanyId = source.Company.Id,
                UseApportionMenu = 1,
                UseAdvanceReceived = 1,
                AdvanceReceivedRecordedDateType = 1,
                ForceShareTransferFee = 1,
            };
            await collationSettingQueryProcessor.SaveAsync(setting, token);
        }

        private async Task InitializeColumnNameSettingAsync(CompanySource source, LoginUser loginUser, CancellationToken token)
        {
            foreach (var tableName in new string[] { nameof(Billing), nameof(Receipt) })
            {
                var noteCount = (tableName == nameof(Billing) ? 8 : 4);
                for (var i = 1; i <= noteCount; i++)
                {
                    var columnName = new ColumnNameSetting
                    {
                        CompanyId = source.Company.Id,
                        TableName = tableName,
                        ColumnName = $"Note{i}",
                        OriginalName = $"備考{(i == 1 ? "" : i.ToString())}",
                        CreateBy = loginUser.Id,
                        UpdateBy = loginUser.Id,
                    };
                    await columnNameSettingQueryProcessor.SaveAsync(columnName, token);
                }
            }
        }

        private async Task InitializeCurrencyAsync(CompanySource source, LoginUser loginUser, CancellationToken token)
        {
            var currency = new Currency
            {
                CompanyId = source.Company.Id,
                Code = VOne.Common.Constants.DefaultCurrencyCode,
                Name = "日本円",
                Symbol = "円",
                Note = "",
                CreateBy = loginUser.Id,
                UpdateBy = loginUser.Id,
            };
            await currencyQueryProcessor.SaveAsync(currency, token);
        }

        private async Task InitializeInvoiceCommonAsync(CompanySource source, LoginUser loginUser, CancellationToken token)
        {
            var invoiceCommonSetting = new InvoiceCommonSetting
            {
                CompanyId = source.Company.Id,
                ExcludeAmountZero = 1,
                ExcludeMinusAmount = 1,
                ExcludeMatchedData = 1,
                CreateBy = loginUser.Id,
                UpdateBy = loginUser.Id,
            };
            await invoiceCommonSettingProcessor.SaveAsync(invoiceCommonSetting, token);
        }

        private async Task InitializeInvoiceNumberAsync(CompanySource source, LoginUser loginUser, CancellationToken token)
        {
            var invoiceNumberSetting = new InvoiceNumberSetting
            {
                CompanyId = source.Company.Id,
                UseNumbering = 0,
                CreateBy = loginUser.Id,
                UpdateBy = loginUser.Id,
            };
            await invoiceNumberSettingProcessor.SaveAsync(invoiceNumberSetting, token);
        }

        private async Task InitializeInvoiceTemplateAsync(CompanySource source, LoginUser loginUser, CancellationToken token)
        {
            var invoiceTemplateSetting = new InvoiceTemplateSetting
            {
                CompanyId = source.Company.Id,
                Code = "00",
                Name = "お振込",
                Title = "請求書",
                Greeting = @"平素は格別のご高配を賜り、厚く御礼申し上げます。
下記の通り請求書を同封いたしましたので、ご査収くださいますよう、よろしくお願いいたします。",
                DisplayStaff = 0,
                DueDateComment = "下記の口座に[YMD]までにご入金していただきますようよろしくお願いいたします。",
                DueDateFormat = 0,
                TransferFeeComment = "なお、手数料は貴社負担とさせていただきますが、ご了承ください。",
                FixedString = string.Empty,
                CreateBy = loginUser.Id,
                UpdateBy = loginUser.Id,
            };
            await invoiceTemplateSettingProcessor.SaveAsync(invoiceTemplateSetting, token);

        }

        private async Task InitializeMasterDataAsync(CompanySource source, CancellationToken token)
        {
            var companyId = source.Company.Id;
            var department = await InitializeDepartmentAsync(source, token);
            var loginUser = await InitializeLoginUserAsync(source, department, token);
            await InitializeLoginUserPasswordAsync(source, loginUser, token);
            await categoryQueryProcessor.InitializeAsync(companyId, loginUser.Id);
            await InitializeCollationSettingAsync(source, token);
            await collationOrderQueryProcessor.InitializeAsync(companyId, loginUser.Id, token);
            await matchingOrderQueryProcessor.InitializeAsync(companyId, loginUser.Id, token);
            await InitializeColumnNameSettingAsync(source, loginUser, token);
            await InitializeCurrencyAsync(source, loginUser, token);
            await generalSettingQueryProcessor.InitializeAsync(companyId, loginUser.Id, token);
            await juridicalPersonalityQueryProcessor.InitializeAsync(companyId, loginUser.Id, token);
            await importSettingQueryProcessor.InitialzieAsync(companyId, loginUser.Id, token);
            await addStatusQueryProcessor.InitializeAsync(companyId, loginUser.Id, token);
            await InitializeInvoiceCommonAsync(source, loginUser, token);

        }

        public async Task<Company> InitializeAsync(CompanySource source, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var company = await addCompanyQueryProcessor.SaveAsync(source.Company, token);
                var companyId = company.Id;
                if (source.Company.Id == 0)
                {
                    source.Company.Id = company.Id;
                    await InitializeMasterDataAsync(source, token);
                }

                if (source.SaveCompanyLogos.Any())
                {
                    foreach (var x in source.SaveCompanyLogos)
                    {
                        x.CompanyId = company.Id;
                        await addCompanyLogoQueryProcessor.SaveAsync(x, token);
                    }
                }

                if (source.DeleteCompanyLogos.Any())
                {
                    foreach (var x in source.DeleteCompanyLogos)
                        await deleteCompanyLogoQueryProcessor.DeleteAsync(companyId, x.LogoType, token);
                }

                if (source.ApplicationControl != null) // これだけはnull(更新対象外)の場合がある。
                {
                    source.ApplicationControl.CompanyId = company.Id;
                    await addApplicationControlQueryProcessor.AddAsync(source.ApplicationControl, token);
                }

                foreach (var x in source.MenuAuthorities)
                {
                    x.CompanyId = company.Id;
                    await addMenuAuthorityQueryProcessor.SaveAsync(x, token);
                }

                foreach (var x in source.FunctionAuthorities)
                {
                    x.CompanyId = company.Id;
                    await addFunctionAuthorityQueryProcessor.SaveAsync(x, token);
                }

                source.PasswordPolicy.CompanyId = company.Id;
                await addPasswordPolicyQueryProcessor.SaveAsync(source.PasswordPolicy, token);

                if (source.LoginUserLicense != null)
                {
                    source.LoginUserLicense.CompanyId = company.Id;
                    await loginUserLicenseQueryProcessor.SaveAsync(source.LoginUserLicense, token);
                }

                scope.Complete();

                return company;
            }
        }

    }
}
