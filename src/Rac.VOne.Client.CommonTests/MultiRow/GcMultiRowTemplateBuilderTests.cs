using NUnit.Framework;
using Rac.VOne.Client.Common.MultiRow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common.MultiRow.Tests
{
    [TestFixture()]
    public class GcMultiRowTemplateBuilderTests
    {
        [TestCase(5, 5, 5)]
        [TestCase(-5, -5, 5)]
        public void GetNumberCellCurrencyTest(int fieldScale, int displayScale, int roundPattern)
        {

            var c = (new GcMultiRowTemplateBuilder()).GetNumberCellCurrency(fieldScale, displayScale, roundPattern);
            Assert.IsNotNull(c);
        }
    }
}