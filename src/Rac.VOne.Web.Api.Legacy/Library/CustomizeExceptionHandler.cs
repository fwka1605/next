using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Library
{
    /// <summary>
    /// カスタマイズ例外処理
    /// </summary>
    public class CustomizeExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandler exceptionHandler;
        /// <summary>constructor</summary>
        public CustomizeExceptionHandler(IExceptionHandler exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;
        }

        /// <summary>例外を非同期的に処理</summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Handle(context);

            return Task.CompletedTask;
            //throw new NotImplementedException();
        }

        /// <summary>例外を同期的に処理</summary>
        /// <param name="context"></param>
        public void Handle(ExceptionHandlerContext context)
        {
            Console.WriteLine(context.Exception);


            context.Result = new InternalServerErrorResult(context.Request);
        }

        ///// <summary>例外を同期的に処理</summary>
        ///// <param name="context"></param>
        //public override void Handle(ExceptionHandlerContext context)
        //{
        //    var request = context.Request;
        //    var exception = context.Exception;

        //    context.Result = new ResponseMessageResult(request.CreateResponse(HttpStatusCode.InternalServerError));

        //    //base.Handle(context);
        //}

        ///// <summary>例外を非同期的に処理</summary>
        ///// <param name="context"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        //{
        //    var request = context.Request;
        //    var exception = context.Exception;

        //    context.Result = new ResponseMessageResult(request.CreateResponse(HttpStatusCode.InternalServerError));


        //    return Task.CompletedTask;
        //    //return base.HandleAsync(context, cancellationToken);
        //}
    }
}