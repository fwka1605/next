using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Batch
{
    class Program
    {
        private const int ResultSuccess = 0;
        private const int ResultError = 1;
        static int Main(string[] args)
        {
            var result = ResultError;
            using (var worker = new Worker(args))
            {
                if (!worker.InitializeResult)
                    return result;

                if (worker.Work())
                    result = ResultSuccess;
            }

            return result;
        }

    }
}
