using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector;
using Rac.VOne.Web.Common.Spreadsheets;
using Rac.VOne.Web.Spreadsheets;

namespace Rac.VOne.Web.Api.Legacy
{
    /// <summary>
    /// DI 登録 Spreadsheet
    /// </summary>
    public class SimpleInjectorSpreadsheetProcessorConfigurator
    {
        private readonly Container container;

        /// <summary>
        /// constructor
        /// </summary>
        public SimpleInjectorSpreadsheetProcessorConfigurator(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// 登録
        /// </summary>
        public void Configure()
        {
            container.Register<IArrearagesListSpreadsheetProcessor, ArrearagesListSpreadsheetProcessor>();
            container.Register<IBillingAgingListSpreadsheetProcessor, BillingAgingListShpreadsheetProcessor>();
        }
    }
}