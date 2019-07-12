using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Rac.VOne.Import;
//using Rac.VOne.Web.Models;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen.Extensions
{
    public static class CsvImporterExtensions
    {
        public static string GetMessageId(this Web.Models.ImportResult result)
            => result.ValidItemCount > 0 && result.InvalidItemCount == 0 ? MsgInfFinishImport
             : result.ValidItemCount > 0 && result.InvalidItemCount > 0 ? MsgInfImportCompletePartOfError
             : result.ValidItemCount == 0 && result.InvalidItemCount == 0 ? MsgErrImportErrorWithLog
             : MsgErrImportErrorWithLog;

        //public static string PadRightMultiByte(this Encoding encoding, string value, int length, char padding = ' ')
        //{
        //    value = value ?? string.Empty;
        //    var byteCount = encoding.GetByteCount(value);
        //    if (length < byteCount)
        //    {
        //        value = new string(value
        //                .TakeWhile((c, i) =>
        //                    encoding.GetByteCount(value.Substring(0, i + 1)) <= length)
        //                .ToArray());
        //        byteCount = encoding.GetByteCount(value);
        //    }
        //    return value.PadRight(length - (byteCount - value.Length), padding);
        //}

        //public static FieldDefinition<TModel> GetDefinition<TModel>(ImporterSettingDetail detail)
        //    where TModel : class
        //{
        //    return null;
        //}
    }

}
