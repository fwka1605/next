using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Rac.VOne.Common.TypeMapping
{
    public interface IAutoMapperTypeConfigurator
    {
        Action<IMapperConfigurationExpression> Configure { get; }
    }
}
