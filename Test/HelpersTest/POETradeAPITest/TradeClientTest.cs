using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.HelpersTest.POETradeAPITest
{
    //TODO add tests
    [TestClass]
    public class TradeClientTest
    {
        // in order to test TradeClient properly, HttpClient will need to be prevented from sending requests
        // usually a mock would suffice, but since GetAsync isn't virtual/abstract, that is not possible
        // my recommendation would be wrapping it in an interface and using that inside TradeClient
    }
}
