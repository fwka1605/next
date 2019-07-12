using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using static Rac.VOne.Web.Common.Constants;

namespace Rac.VOne.Web.Api.Legacy.Extensions
{
    /// <summary>
    /// 帳票用
    /// </summary>
    public static class HttpRequestMessageExtension
    {
        /// <summary>
        /// <see cref="HttpRequestMessage"/>から、帳票用<see cref="HttpResponseMessage"/>を返す
        /// </summary>
        /// <param name="request"></param>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static HttpResponseMessage GetResponseMessage(this HttpRequestMessage request,
            byte[] content,
            string fileName = "sample.pdf",
            string mediaType = "application/pdf")
        {
            var response = request.CreateResponse();

            if (content == null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NoContent;
                return response;
            }

            response.Content = new ByteArrayContent(content);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attatchment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return response;
        }

        /// <summary>
        /// <see cref="HttpRequestMessage"/>から、Spreadsheet用<see cref="HttpResponseMessage"/>を返す
        /// </summary>
        /// <param name="request"></param>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static HttpResponseMessage GetSpreadsheetResponseMessage(this HttpRequestMessage request,
            byte[] content,
            string fileName)
            => request.GetResponseMessage(content, fileName, SpreadsheetContentType);
    }
}