using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class ControlColorProcessor :IControlColorProcessor
    {
        private readonly IAutoMapper autoMapper;
        private readonly IControlColorQueryProcessor controlColorQueryProcessor ;
        private readonly IAddControlColorQueryProcessor addControlColorQueryProcessor;

        public ControlColorProcessor(
            IAutoMapper autoMapper,
            IControlColorQueryProcessor controlColorQueryProcessor,
            IAddControlColorQueryProcessor addControlColorQueryProcessor
            )
        {
            this.autoMapper = autoMapper;
            this.controlColorQueryProcessor = controlColorQueryProcessor;
            this.addControlColorQueryProcessor = addControlColorQueryProcessor;
        }

        public async Task<ControlColor> GetAsync(int CompanyId, int LoginUserId, CancellationToken token = default(CancellationToken))
        {
            var colorEntity = await controlColorQueryProcessor.GetAsync(CompanyId, LoginUserId);
            return autoMapper.Map<ControlColor>(colorEntity);
        }

        public async Task<ControlColor> SaveAsync(ControlColor ControlColor, CancellationToken token = default(CancellationToken))
        {
            var entity = autoMapper.Map<Data.Entities.ControlColor>(ControlColor);
            var entityResult = await addControlColorQueryProcessor.SaveAsync(entity);
            return autoMapper.Map<ControlColor>(entityResult);
        }
    }
}
