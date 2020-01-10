using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sidekick.Helpers;

namespace Test.HelpersTest
{
    [TestClass]
    public class ItemParserTest
    {
        //TODO add test cases
        //Item will need to be made IEquatable<Item> for the tests to work
        [DataTestMethod]
        [DataRow("", null)]
        public void ParseItem_WithInput_IsExpected(string input, Item expected)
        {
            var result = ItemParser.ParseItem(input);
            Assert.AreEqual(expected, result);
        }
    }
}
