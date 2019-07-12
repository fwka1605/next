using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;

namespace Rac.VOne.Client.Common.MultiRow
{
    public class CellSetting
    {
        public string Name { get; set; }
        /// <summary>
        /// ヘッダーセルのキャプション
        /// 行<see cref="Row"/>のみを設定する場合は 設定不要
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// データバインドする項目 Model の プロパティ名を設定
        /// 紐付けを持たせるため、 nameof 演算子の利用を行うこと
        /// </summary>
        public string DataField { get; set; }
        public Cell CellInstance { get; set; }
        /// <summary>
        ///  セルの読み取り専用設定 初期値は <see cref="true"/> 読み取り専用
        ///  ヘッダーセルを設定する場合は 設定不要
        ///  入力項目 初期値で入力可とする場合だけ、<see cref="false"/>を設定すること
        /// </summary>
        public bool ReadOnly { get; set; } = true;
        /// <summary>
        /// 編集可否の設定　初期値は <see cref="true"/>
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// 表示要否の設定 初期値は <see cref="true"/>
        /// </summary>
        public bool Visible { get; set; } = true;
        /// <summary>
        /// <see cref="Cell.TabStop"/> の設定を行う 初期値は <see cref="true"/>
        /// </summary>
        public bool TabStop { get; set; } = true;
        /// <summary>
        /// <see cref="Cell.TabIndex"/> の設定を行う
        /// </summary>
        public int TabIndex { get; set; } = 0;
        public Size Size { get; set; }
        public object Value { get; set; }
        public int Width
        {
            get { return Size.Width; }
            set { Size = new Size(value, Size.Height); }
        }
        public int Height
        {
            get { return Size.Height; }
            set { Size = new Size(Size.Width, value); }
        }
        public Point Location { get; set; }
        public int X
        {
            get { return Location.X; }
            set { Location = new Point(value, Location.Y); }
        }
        public int Y
        {
            get { return Location.Y; }
            set { Location = new Point(Location.X, value); }
        }
        public Color BackColor { get; set; }
        public Color SelectionBackColor { get; set; }
        public Color DisableBackColor { get; set; } = Color.WhiteSmoke;
        public Border Border { get; set; }
        public Font Font { get; set; }
        /// <summary>
        /// ヘッダーを作成する場合に有効 グリッド標準の ソート機能を付加するか否か
        /// 初期値は <see cref="false"/>
        /// </summary>
        public bool SortDropDown { get; set; } = false;
        public HeaderDropDownList DropDownList { get; set; } = null;

        public bool Selectable { get; set; } = true;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="height">cell の高さ</param>
        /// <param name="width">cell の幅</param>
        /// <param name="name">cell の名称 指定した name の前に cel という prefixが付く header は lbl</param>
        /// <param name="dataField">bind する DataField 文字列指定</param>
        /// <param name="caption">header の caption</param>
        /// <param name="readOnly">読取専用 default: true 入力用の cell は false を指定すること</param>
        /// <param name="enabled">有効無効 default: true</param>
        /// <param name="visible">表示有無 default: true 非表示用項目を追加する場合は false を明示</param>
        /// <param name="tabStop">TabStop 有効無効 default: true</param>
        /// <param name="tabIndex">セル内のタブインデックス</param>
        /// <param name="value">初期値</param>
        /// <param name="cell">cell instance 対象の cell のインスタンスを渡す 指定しない場合は GcTextBoxCell の標準となる</param>
        /// <param name="sortable">header にソート用のドロップダウンリスト（標準）を表示するかどうか</param>
        /// <param name="dropDown">header に独自のドロップダウンリストを表示する場合 HeaderDropDownList を渡す</param>
        /// <param name="location">cell の locationを明示して指定する場合 通常は、height, width と登録された順番で指定する</param>
        /// <param name="border">cell border を独自に設定したい場合に指定</param>
        /// <param name="backColor">通常時の背景色</param>
        /// <param name="selectionBackColor">選択時の背景色</param>
        /// <param name="disableBackColor">無効時の背景色</param>
        /// <param name="font">フォント グリッドの設定ではなく、セル個別設定時に指定</param>
        public CellSetting(int height, int width, string name,
            string dataField = "",
            string caption = "",
            bool readOnly = true,
            bool enabled = true,
            bool visible = true,
            bool tabStop = true,
            int tabIndex = 0,
            object value = null,
            Cell cell = null,
            bool sortable = false,
            HeaderDropDownList dropDown = null,
            Point? location = null,
            Border border = null,
            Color? backColor = null,
            Color? selectionBackColor = null,
            Color? disableBackColor = null,
            Font font = null,
            bool selectable = true)
        {
            Height = height;
            Width = width;
            Name = name;
            DataField = dataField;
            Caption = caption;
            ReadOnly = readOnly;
            Enabled = enabled;
            Visible = visible;
            TabStop = tabStop;
            TabIndex = tabIndex;
            Value = value;
            CellInstance = cell;
            SortDropDown = sortable;
            DropDownList = dropDown;
            if (location.HasValue)
                Location = location.Value;
            Border = border;
            BackColor = backColor ?? Color.Empty;
            SelectionBackColor = selectionBackColor ?? Color.Empty;
            DisableBackColor = disableBackColor ?? Color.WhiteSmoke;
            Font = font;
            Selectable = selectable;
        }

    }
}
