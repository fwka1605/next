using AutoMapper;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Web.Models;
using System;
using System.Drawing;

namespace Rac.VOne.Web.Common.AutoMappingConfiguration
{
    public class ControlColorTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public Action<IMapperConfigurationExpression> Configure { get; }
            = cfg =>
            {
                cfg.CreateMap<Data.Entities.ControlColor, ControlColor>()
                    .ForMember(m => m.FormBackColor                             , opt => opt.MapFrom(e => Color.FromArgb(e.FormBackColor)))
                    .ForMember(m => m.FormForeColor                             , opt => opt.MapFrom(e => Color.FromArgb(e.FormForeColor)))
                    .ForMember(m => m.ControlEnableBackColor                    , opt => opt.MapFrom(e => Color.FromArgb(e.ControlEnableBackColor)))
                    .ForMember(m => m.ControlDisableBackColor                   , opt => opt.MapFrom(e => Color.FromArgb(e.ControlDisableBackColor)))
                    .ForMember(m => m.ControlForeColor                          , opt => opt.MapFrom(e => Color.FromArgb(e.ControlForeColor)))
                    .ForMember(m => m.ControlRequiredBackColor                  , opt => opt.MapFrom(e => Color.FromArgb(e.ControlRequiredBackColor)))
                    .ForMember(m => m.ControlActiveBackColor                    , opt => opt.MapFrom(e => Color.FromArgb(e.ControlActiveBackColor)))
                    .ForMember(m => m.ButtonBackColor                           , opt => opt.MapFrom(e => Color.FromArgb(e.ButtonBackColor)))
                    .ForMember(m => m.GridRowBackColor                          , opt => opt.MapFrom(e => Color.FromArgb(e.GridRowBackColor)))
                    .ForMember(m => m.GridAlternatingRowBackColor               , opt => opt.MapFrom(e => Color.FromArgb(e.GridAlternatingRowBackColor)))
                    .ForMember(m => m.GridLineColor                             , opt => opt.MapFrom(e => Color.FromArgb(e.GridLineColor)))
                    .ForMember(m => m.InputGridBackColor                        , opt => opt.MapFrom(e => Color.FromArgb(e.InputGridBackColor)))
                    .ForMember(m => m.InputGridAlternatingBackColor             , opt => opt.MapFrom(e => Color.FromArgb(e.InputGridAlternatingBackColor)))
                    .ForMember(m => m.MatchingGridBillingBackColor              , opt => opt.MapFrom(e => Color.FromArgb(e.MatchingGridBillingBackColor)))
                    .ForMember(m => m.MatchingGridReceiptBackColor              , opt => opt.MapFrom(e => Color.FromArgb(e.MatchingGridReceiptBackColor)))
                    .ForMember(m => m.MatchingGridBillingSelectedRowBackColor   , opt => opt.MapFrom(e => Color.FromArgb(e.MatchingGridBillingSelectedRowBackColor)))
                    .ForMember(m => m.MatchingGridBillingSelectedCellBackColor  , opt => opt.MapFrom(e => Color.FromArgb(e.MatchingGridBillingSelectedCellBackColor)))
                    .ForMember(m => m.MatchingGridReceiptSelectedRowBackColor   , opt => opt.MapFrom(e => Color.FromArgb(e.MatchingGridReceiptSelectedRowBackColor)))
                    .ForMember(m => m.MatchingGridReceiptSelectedCellBackColor  , opt => opt.MapFrom(e => Color.FromArgb(e.MatchingGridReceiptSelectedCellBackColor)));

               cfg.CreateMap<ControlColor, Data.Entities.ControlColor>()
                    .ForMember(e => e.FormBackColor                             , opt => opt.MapFrom(m => m.FormBackColor.ToArgb()))
                    .ForMember(e => e.FormForeColor                             , opt => opt.MapFrom(m => m.FormForeColor.ToArgb()))
                    .ForMember(e => e.ControlEnableBackColor                    , opt => opt.MapFrom(m => m.ControlEnableBackColor.ToArgb()))
                    .ForMember(e => e.ControlDisableBackColor                   , opt => opt.MapFrom(m => m.ControlDisableBackColor.ToArgb()))
                    .ForMember(e => e.ControlForeColor                          , opt => opt.MapFrom(m => m.ControlForeColor.ToArgb()))
                    .ForMember(e => e.ControlRequiredBackColor                  , opt => opt.MapFrom(m => m.ControlRequiredBackColor.ToArgb()))
                    .ForMember(e => e.ControlActiveBackColor                    , opt => opt.MapFrom(m => m.ControlActiveBackColor.ToArgb()))
                    .ForMember(e => e.ButtonBackColor                           , opt => opt.MapFrom(m => m.ButtonBackColor.ToArgb()))
                    .ForMember(e => e.GridRowBackColor                          , opt => opt.MapFrom(m => m.GridRowBackColor.ToArgb()))
                    .ForMember(e => e.GridAlternatingRowBackColor               , opt => opt.MapFrom(m => m.GridAlternatingRowBackColor.ToArgb()))
                    .ForMember(e => e.GridLineColor                             , opt => opt.MapFrom(m => m.GridLineColor.ToArgb()))
                    .ForMember(e => e.InputGridBackColor                        , opt => opt.MapFrom(m => m.InputGridBackColor.ToArgb()))
                    .ForMember(e => e.InputGridAlternatingBackColor             , opt => opt.MapFrom(m => m.InputGridAlternatingBackColor.ToArgb()))
                    .ForMember(e => e.MatchingGridBillingBackColor              , opt => opt.MapFrom(m => m.MatchingGridBillingBackColor.ToArgb()))
                    .ForMember(e => e.MatchingGridReceiptBackColor              , opt => opt.MapFrom(m => m.MatchingGridReceiptBackColor.ToArgb()))
                    .ForMember(e => e.MatchingGridBillingSelectedRowBackColor   , opt => opt.MapFrom(m => m.MatchingGridBillingSelectedRowBackColor.ToArgb()))
                    .ForMember(e => e.MatchingGridBillingSelectedCellBackColor  , opt => opt.MapFrom(m => m.MatchingGridBillingSelectedCellBackColor.ToArgb()))
                    .ForMember(e => e.MatchingGridReceiptSelectedRowBackColor   , opt => opt.MapFrom(m => m.MatchingGridReceiptSelectedRowBackColor.ToArgb()))
                    .ForMember(e => e.MatchingGridReceiptSelectedCellBackColor  , opt => opt.MapFrom(m => m.MatchingGridReceiptSelectedCellBackColor.ToArgb()));
        };

    }
}
