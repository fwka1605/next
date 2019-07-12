using System.Drawing;
using Rac.VOne.Client.Common.MultiRow;

namespace Rac.VOne.Client.Common
{
    public static class IApplicationExtension
    {
        /// <summary>Template Builder のインスタンスを生成する拡張メソッド</summary>
        /// <param name="app"></param>
        /// <param name="color"></param>
        /// <param name="autoLocationSet"></param>
        /// <param name="allowHorizontalResize"></param>
        /// <param name="sortable"></param>
        /// <returns></returns>
        public static GcMultiRowTemplateBuilder CreateGcMultirowTemplateBuilder(
            this IApplication app,
            IColors color,
            bool autoLocationSet = true,
            bool allowHorizontalResize = true,
            bool sortable = false)
        {
            var builer = new GcMultiRowTemplateBuilder()
            {
                AutoLocationSet         = autoLocationSet,
                AllowHorizontalResize   = allowHorizontalResize,
                Sortable                = sortable,
                Font                    = new Font(app.FontFamilyName, (float)9.0, FontStyle.Regular, GraphicsUnit.Point, (byte)128),
                Border                  = color.GridLine(),
            };
            return builer;
        }
    }
}
