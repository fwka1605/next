using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;

namespace Rac.VOne.Web.Spreadsheets
{
    public static class OpenXmlSpreadsheetDrawingExtensions
    {

        public static Xdr.FromMarker GetFromMarker(this IMarker marker)
        {
            return new Xdr.FromMarker {
                ColumnId        = new ColumnId      (marker.ColumnId),
                RowId           = new RowId         (marker.RowId),
                ColumnOffset    = new ColumnOffset  (marker.ColumnOffset),
                RowOffset       = new RowOffset     (marker.RowOffset),
            };
        }
        public static Xdr.ToMarker GetToMarker(this IMarker marker)
        {
            return new Xdr.ToMarker {
                ColumnId        = new ColumnId      (marker.ColumnId),
                RowId           = new RowId         (marker.RowId),
                ColumnOffset    = new ColumnOffset  (marker.ColumnOffset),
                RowOffset       = new RowOffset     (marker.RowOffset),
            };
        }


        public static OpenXmlElement GetAnchor(this IPicture pic)
        {
            return pic.To == null ? (OpenXmlElement)
                GetOneCellAnchor(pic) :
                GetTwoCellAnchor(pic);
        }

        private static OneCellAnchor GetOneCellAnchor(IPicture pic)
        {
            return new OneCellAnchor(
                pic.From.GetFromMarker(),
                new Xdr.Extent {
                    Cx          = pic.ExtentCx,
                    Cy          = pic.ExtentCy,
                },
                new Xdr.Picture(
                    new Xdr.NonVisualPictureProperties(
                        new Xdr.NonVisualDrawingProperties { Id = pic.PropertyId, Name = pic.PropertyName },
                        new Xdr.NonVisualPictureDrawingProperties(new PictureLocks { NoChangeAspect = true })
                    ),
                    new Xdr.BlipFill(
                        new Blip { Embed = pic.EmbedId, CompressionState = BlipCompressionValues.Print },
                        new Stretch(new FillRectangle())
                    ),
                    new Xdr.ShapeProperties(
                        new Transform2D(
                            new Offset { X = 0, Y = 0 },
                            new Extents { Cx = pic.ExtentCx, Cy = pic.ExtentCy }
                        ),
                        new PresetGeometry { Preset = ShapeTypeValues.Rectangle }
                    )
                ),
                new Xdr.ClientData()
            );
        }

        private static TwoCellAnchor GetTwoCellAnchor(IPicture pic)
        {
            return new TwoCellAnchor(
                pic.From.GetFromMarker(),
                pic.To  .GetToMarker(),
                new Xdr.Picture(
                    new Xdr.NonVisualPictureProperties(
                        new Xdr.NonVisualDrawingProperties { Id = pic.PropertyId, Name = pic.PropertyName },
                        new Xdr.NonVisualPictureDrawingProperties(new PictureLocks { NoChangeAspect = true })
                    ),
                    new Xdr.BlipFill(
                        new Blip { Embed = pic.EmbedId, CompressionState = BlipCompressionValues.Print },
                        new Stretch(new FillRectangle())
                    ),
                    new Xdr.ShapeProperties(
                        new Transform2D(
                            new Offset { X = 0, Y = 0 },
                            new Extents { Cx = pic.ExtentCx, Cy = pic.ExtentCy }
                        ),
                        new PresetGeometry { Preset = ShapeTypeValues.Rectangle }
                    )
                ),
                new Xdr.ClientData()
            );
        }


        /// <summary>
        /// EMU: English Metric Units
        /// centimeter / inch / point それぞれで 小数点が発生しにくい単位としている
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        /// <remarks>
        /// http://polymathprogrammer.com/2009/10/22/english-metric-units-and-open-xml/
        /// https://web.archive.org/web/20180428194217/http://archive.oreilly.com/pub/post/what_is_an_emu.html
        /// https://en.wikipedia.org/wiki/Office_Open_XML_file_formats#DrawingML
        /// </remarks>
        public static long ConvertToEMU(int pixels, double resolution)
            => Convert.ToInt64(914400L * pixels / resolution);

        public static long GetWidthEMU(this int pixels)
            => ConvertToEMU(pixels, Graphics.DpiX);

        public static long GetHeightEMU(this int pixels)
            => ConvertToEMU(pixels, Graphics.DpiY);

        public static long GetWidthEMU(this Bitmap bitmap)
            => ConvertToEMU(bitmap.Width, Graphics.DpiX);

        public static long GetHeightEMU(this Bitmap bitmap)
            => ConvertToEMU(bitmap.Height, Graphics.DpiY);

        private static Graphics threadLocalGraphics;
        internal static Graphics Graphics
        {
            get
            {
                if (threadLocalGraphics == null)
                {
                    threadLocalGraphics = Graphics.FromImage(new Bitmap(1, 1));
                }
                return threadLocalGraphics;
            }
        }

        public static string ToInvaliantString(this object value)
        {
            var info = System.Globalization.CultureInfo.InvariantCulture;
            if (value == null) return string.Empty;
            switch (value)
            {
                case TimeSpan ts:
                    return ts.ToString("c", info);
                case int v:
                    return v.ToString(info);
                case uint v:
                    return v.ToString(info);
                case long v:
                    return v.ToString(info);
                case ulong v:
                    return v.ToString(info);
                default:
                    return value.ToString();
            }
        }

        /// <summary>
        /// <see cref="ImagePartType"/> の取得 <see cref="System.Drawing.Bitmap"/>の RawFormat から Guid を取得して比較
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static ImagePartType GetImagePartType(this Bitmap bitmap)
        {
            var guid = bitmap.RawFormat.Guid;
            if (ImageFormat.Bmp .Guid   == guid) return ImagePartType.Bmp;
            if (ImageFormat.Jpeg.Guid   == guid) return ImagePartType.Jpeg;
            if (ImageFormat.Png .Guid   == guid) return ImagePartType.Png;
            if (ImageFormat.Gif .Guid   == guid) return ImagePartType.Gif;
            if (ImageFormat.Tiff.Guid   == guid) return ImagePartType.Tiff;
            if (ImageFormat.Icon.Guid   == guid) return ImagePartType.Icon;

            throw new ArgumentException($"Not supported image format {bitmap.RawFormat.Guid}");
        }

    }
}
