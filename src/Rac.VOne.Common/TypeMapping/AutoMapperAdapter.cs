using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace Rac.VOne.Common.TypeMapping
{
    public class AutoMapperAdapter : IAutoMapper
    {
        private IMapper mapper;

        public void Initialize(IEnumerable<IAutoMapperTypeConfigurator> configurators)
        {
            var config = new MapperConfiguration(cfg => {
                foreach (var configurator in configurators)
                    configurator?.Configure?.Invoke(cfg);
            });
            mapper = config.CreateMapper();
        }
        public T Map<T>(object objectToMap)
        {
            return mapper.Map<T>(objectToMap);
        }

        public TExist Map<TSource, TExist>(TSource from, TExist to)
        {
            return mapper.Map<TSource, TExist>(from, to);
        }
    }

    public static class AutoMapperExtensions
    {
        /// <summary>
        /// <see cref="IEnumerable{T}"/>をModelTo へすべて Map
        /// </summary>
        /// <typeparam name="ModelTo"></typeparam>
        /// <param name="autoMapper"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<ModelTo> MapAll<ModelTo>(this IAutoMapper autoMapper, IEnumerable<object> source)
        {
            return source.Select(x => autoMapper.Map<ModelTo>(x));
        }
    }
}
