using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Entities = Rac.VOne.Data.Entities;

namespace Rac.VOne.Web.Common
{
    public class SessionStorageProcessor : ISessionStorageProcessor
    {
        private readonly IAutoMapper autoMapper;
        private readonly ISessionStorageQueryProcessor sessionStorageQueryProcessor;
        private readonly IStringConnectionFactory stringConnectionFactory; // BranchではなくTrunkに接続する必要があるため

        public SessionStorageProcessor(
            IAutoMapper mapper,
            ISessionStorageQueryProcessor sessionStorage,
            IStringConnectionFactory connectionFactory)
        {
            autoMapper = mapper;
            sessionStorageQueryProcessor = sessionStorage;
            stringConnectionFactory = connectionFactory;
        }

        public async Task<Session> GetAsync(string SessionKey, CancellationToken token = default(CancellationToken))
        {
            var trunkConnectionSettings = ConfigurationManager.ConnectionStrings["VOne.Scarlet"];
            var trunkConnectionFactory = stringConnectionFactory.Create(trunkConnectionSettings.ConnectionString);

            var entity = await sessionStorageQueryProcessor.GetAsync(trunkConnectionFactory, SessionKey, token);
            return autoMapper.Map<Session>(entity);
        }

    }
}
