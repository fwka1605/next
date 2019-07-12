using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Web.Common.AutoMappingConfiguration;

namespace Rac.VOne.Web.Api.Extensions
{

    /// <summary>
    /// automapper の config を実施する拡張メソッド
    /// </summary>
    public static class AutoMapperConfigurationExtension
    {
        /// <summary>初期化</summary>
        public static void Initialize(this AutoMapperAdapter mapper)
            => mapper.Initialize(new IAutoMapperTypeConfigurator[]
            {
                new ControlColorTypeConfigurator(),
                new SessionTypeConfigurator(),
            });

    }
}
