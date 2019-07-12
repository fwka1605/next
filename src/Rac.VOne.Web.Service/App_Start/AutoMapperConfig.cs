using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Web.Common.AutoMappingConfiguration;

namespace Rac.VOne.Web.Service
{
    public static class AutoMapperConfig
    {
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