using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Web.Common
{
    public class EBFileSettingProcessor : IEBFileSettingProcessor
    {
        private readonly IEBFileSettingQueryProcessor ebFileSettingQueryProcessor;
        private readonly IAddEBFileSettingQueryProcessor addEbFileSettingQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<EBFileSetting> ebFileSettingDeleteQueryProcessor;
        private readonly IUpdateEBFileSettingQueryProcessor updateEBFileSettingQueryProcessor;

        public EBFileSettingProcessor(
            IEBFileSettingQueryProcessor ebFileSettingQueryProcessor,
            IAddEBFileSettingQueryProcessor addEbFileSettingQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<EBFileSetting> ebFileSettingDeleteQueryProcessor,
            IUpdateEBFileSettingQueryProcessor updateEBFileSettingQueryProcessor
            )
        {
            this.ebFileSettingQueryProcessor = ebFileSettingQueryProcessor;
            this.addEbFileSettingQueryProcessor = addEbFileSettingQueryProcessor;
            this.ebFileSettingDeleteQueryProcessor = ebFileSettingDeleteQueryProcessor;
            this.updateEBFileSettingQueryProcessor = updateEBFileSettingQueryProcessor;
        }

        public async Task<IEnumerable<EBFileSetting>> GetAsync(EBFileSettingSearch option, CancellationToken token = default(CancellationToken))
            => await ebFileSettingQueryProcessor.GetAsync(option, token);

        public async Task<int> DeleteAsync(int id, CancellationToken token = default(CancellationToken))
            => await ebFileSettingDeleteQueryProcessor.DeleteAsync(id, token);

        public async Task<EBFileSetting> SaveAsync(EBFileSetting setting, CancellationToken token = default(CancellationToken))
            => await addEbFileSettingQueryProcessor.SaveAsync(setting, token);


        public async Task<int> UpdateIsUseableAsync(int CompanyId, int LoginUserId, IEnumerable<int> ids, CancellationToken token = default(CancellationToken))
            => await updateEBFileSettingQueryProcessor.UpdateIsUseableAsync(CompanyId, LoginUserId, ids, token);

    }
}
