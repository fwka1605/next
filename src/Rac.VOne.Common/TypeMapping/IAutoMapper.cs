using System.Collections.Generic;

namespace Rac.VOne.Common.TypeMapping
{
    public interface IAutoMapper
    {
        void Initialize(IEnumerable<IAutoMapperTypeConfigurator> configurators);

        T Map<T>(object objectToMap);

        TExist Map<TSource, TExist>(TSource from, TExist to);
    }
}
