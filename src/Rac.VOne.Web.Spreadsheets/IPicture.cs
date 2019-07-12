namespace Rac.VOne.Web.Spreadsheets
{
    public interface IPicture
    {
        /// <summary>_rels で 定義される rId</summary>
        string EmbedId { get; set; }
        /// <summary>NonvisualDrawingProperties Id 画像ごとに1から始まるId</summary>
        uint PropertyId { get; set; }
        /// <summary>NonvisualDrawingProperties Name 画像の名前</summary>
        string PropertyName { get; set; }
        /// <summary>描画領域を規定する開始位置</summary>
        IMarker From { get; set; }
        /// <summary>描画領域を規定する終了位置</summary>
        IMarker To { get; set; }
        /// <summary>English Metric Units の width</summary>
        long ExtentCx { get; set; }
        /// <summary>English Metric Units の height</summary>
        long ExtentCy { get; set; }
    }
}
