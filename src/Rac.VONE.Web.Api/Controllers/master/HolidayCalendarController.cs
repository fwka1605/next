using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  祝日カレンダーマスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HolidayCalendarController : ControllerBase
    {

        private readonly IHolidayCalendarProcessor holidaycalendarProcessor;
        private readonly IHolidayCalendarFileImportProcessor holidayCalendarImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public HolidayCalendarController(
            IHolidayCalendarProcessor holidaycalendarProcessor,
            IHolidayCalendarFileImportProcessor holidayCalendarImportProcessor
            )
        {
            this.holidaycalendarProcessor = holidaycalendarProcessor;
            this.holidayCalendarImportProcessor = holidayCalendarImportProcessor;
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="holidays">祝日 配列</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(IEnumerable<HolidayCalendar> holidays, CancellationToken token)
            => await holidaycalendarProcessor.DeleteAsync(holidays, token);

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索項目</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HolidayCalendar>>> GetItems(HolidayCalendarSearch option, CancellationToken token)
            => (await holidaycalendarProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="holidays">祝日カレンダー</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HolidayCalendar>>> Save(IEnumerable<HolidayCalendar> holidays, CancellationToken token)
            => (await holidaycalendarProcessor.SaveAsync(holidays, token)).ToArray();

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImportResult>> Import(MasterImportSource source, CancellationToken token)
            => await holidayCalendarImportProcessor.ImportAsync(source, token);

    }
}
