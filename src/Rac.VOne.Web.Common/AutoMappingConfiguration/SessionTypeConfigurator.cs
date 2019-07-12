using AutoMapper;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Web.Models;
using System;

namespace Rac.VOne.Web.Common.AutoMappingConfiguration
{
    public class SessionTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public Action<IMapperConfigurationExpression> Configure { get; }
           = cfg =>
           {
               cfg.CreateMap<Data.Entities.SessionStorage, Session>();
               cfg.CreateMap<Session, Data.Entities.SessionStorage>();
           };
    }
}