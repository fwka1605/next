using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.EbData
{
    public class LineSplitterJPBank : ILineSplitter
    {
        public string[] Split(string line)
        {
            if (string.IsNullOrEmpty(line))
                return null;
            line = line.PadRight(128, ' ');
            if (line.StartsWith(Constants.DataKubun.Header))
                return new string[]
                {
                    line.Substring(  0,   1),
                    line.Substring(  1,   3),
                    line.Substring(  4,   5),
                    line.Substring(  9,   8),
                    line.Substring( 17,   8),
                    line.Substring( 25,   1),
                    line.Substring( 26, 102),
                };
            if (line.StartsWith(Constants.DataKubun.Data))
                return new string[]
                {
                    line.Substring(  0,   1),
                    line.Substring(  1,   3),
                    line.Substring(  4,   1),
                    line.Substring(  5,   4),
                    line.Substring(  9,   5),
                    line.Substring( 14,  10),
                    line.Substring( 24,   5),
                    line.Substring( 29,   5),
                    line.Substring( 34,   1),
                    line.Substring( 35,   8),
                    line.Substring( 43,  60),
                    line.Substring(103,  25),
                };
            if (line.StartsWith(Constants.DataKubun.Detail))
                return new string[]
                {
                    line.Substring(  0,   1),
                    line.Substring(  1,   1),
                    line.Substring(  2,   2),
                    line.Substring(  4, 120),
                    line.Substring(124,   4),
                };
            if (line.StartsWith(Constants.DataKubun.Trailer))
                return new string[]
                {
                    line.Substring(  0,   1),
                    line.Substring(  1,   3),
                    line.Substring(  4,   6),
                    line.Substring( 10,  12),
                    line.Substring( 22,   6),
                    line.Substring( 28,  12),
                    line.Substring( 40,  88),
                };
            if (line.StartsWith(Constants.DataKubun.End))
                return new string[]
                {
                    line.Substring(  0,   1),
                    line.Substring(  1, 127),
                };
            return new string[] { };
        }
    }
}
