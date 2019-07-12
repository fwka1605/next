using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  �j���J�����_�[�}�X�^�[
    /// </summary>
    public class HolidayCalendarController : ApiControllerAuthorized
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
        /// �폜
        /// </summary>
        /// <param name="holidays">�j�� �z��</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(IEnumerable<HolidayCalendar> holidays, CancellationToken token)
            => await holidaycalendarProcessor.DeleteAsync(holidays, token);

        /// <summary>
        /// �擾 �z��
        /// </summary>
        /// <param name="option">��������</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<HolidayCalendar>> GetItems(HolidayCalendarSearch option, CancellationToken token)
            => (await holidaycalendarProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// �o�^
        /// </summary>
        /// <param name="holidays">�j���J�����_�[</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<HolidayCalendar>> Save(IEnumerable<HolidayCalendar> holidays, CancellationToken token)
            => (await holidaycalendarProcessor.SaveAsync(holidays, token)).ToArray();

        /// <summary>
        /// �C���|�[�g
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await holidayCalendarImportProcessor.ImportAsync(source, token);

    }
}
