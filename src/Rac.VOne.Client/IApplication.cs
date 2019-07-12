
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client
{
    public interface IApplication
    {
        Form Create(string className);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="subForm"></param>
        /// <param name="centerParent">
        /// true: subForm を owner ウィンドウの中央に表示する。サイズ調整なし。<para/>
        /// false: subForm を owner と同じ位置・サイズで表示する。
        /// </param>
        /// <returns></returns>
        DialogResult ShowDialog(IWin32Window owner, Form subForm, bool centerParent = false);

        DialogResult ShowPreviewForm(IWin32Window owner, GrapeCity.ActiveReports.SectionReport report);

        ILogin Login { get; set; }

        string FontFamilyName { get; set; }
    }
}
