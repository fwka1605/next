using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Web.Common.AutoMappingConfiguration;

namespace Rac.VOne.Web.Api.Legacy
{
    /// <summary>
    /// automapper の構成
    /// </summary>
    public static class AutoMapperConfig
    {
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static AutoMapperAdapter Initialize(AutoMapperAdapter mapper)
        {
            mapper.Initialize(new IAutoMapperTypeConfigurator[]
            {
                new ControlColorTypeConfigurator(),
                new SessionTypeConfigurator(),
            });
            return mapper;
        }
    }
}