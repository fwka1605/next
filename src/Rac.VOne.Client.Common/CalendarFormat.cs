using System.ComponentModel;
namespace Rac.VOne.Client.Common
{
    /// <summary>
    /// 西暦/和暦のフォーマット
    /// </summary>
    [DefaultValue(GregorianCalendar)]
    public enum CalendarFormat
    {
        /// <summary>
        /// 0 : 西暦(グレゴリオ暦)
        /// </summary>
        GregorianCalendar = 0,
        /// <summary>
        /// 1 : 和暦
        /// </summary>
        JapaneseCalendar
    }
}
