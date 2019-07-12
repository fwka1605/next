using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.EbData
{
    public class LineSplitterZenginNyusyukkin : ILineSplitter
    {
        private static class FieldSizeInfo
        {
            public static int[] Header = new int[] { 1, 2, 1, 6, 6, 6, 4, 15, 3, 15, 3, 1, 10, 40, 1, 1, 14, 71 };
            public static int[] Data = new int[] { 1, 8, 6, 6, 1, 2, 12, 12, 6, 6, 1, 7, 3, 10, 48, 15, 15, 20, 20, 1 };
            public static int[] Trailer = new int[] { 1, 6, 13, 6, 13, 1, 14, 7, 139 };
            public static int[] End = new int[] { 1, 10, 5, 184 };
        }

        public string[] Split(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            line = line.PadRight(200, ' ');

            switch (/*データ区分*/line[0].ToString())
            {
                case Constants.DataKubun.Header:
                    return ExtractFields(line, FieldSizeInfo.Header);

                case Constants.DataKubun.Data:
                    return ExtractFields(line, FieldSizeInfo.Data);

                case Constants.DataKubun.Trailer:
                    return ExtractFields(line, FieldSizeInfo.Trailer);

                case Constants.DataKubun.End:
                    return ExtractFields(line, FieldSizeInfo.End);
            }

            return new string[] { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="fieldSizeList"></param>
        /// <returns></returns>
        /// <remarks>
        /// record長はフィールド展開に十分な長さを持っている前提。
        /// </remarks>
        private static string[] ExtractFields(string record, int[] fieldSizeList)
        {
            var fieldList = new List<string>();

            if (string.IsNullOrEmpty(record))
            {
                return null;
            }

            var idx = 0;
            foreach (var size in fieldSizeList)
            {
                fieldList.Add(record.Substring(idx, size));
                idx += size;
            };

            //if (idx <= record.Length)
            //{
            //    fieldList.Add(record.Substring(idx)); // 残り全て
            //}

            return fieldList.ToArray();
        }
    }
}
