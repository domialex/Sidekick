using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sidekick.Helpers.POENinjaAPI;
using Sidekick.Helpers.POENinjaAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Tests.POENinjaAPI
{
    [TestClass]
    class NinjaClientTests
    {
        private NinjaClient _sut;

        [TestInitialize]
        public void Init()
        {
            _sut = new NinjaClient();
        }


        [TestMethod]
        [DataRow("Metamorph")]
        public async Task CanGetItemOverviews(string league)
        {
            foreach (var itemType in Enum.GetValues(typeof(ItemType)))
            {
                var actualResponse = await _sut.GetItemOverview(league, (ItemType)itemType);
                Assert.IsNotNull(actualResponse);
            }
        }

        [TestMethod]
        [DataRow("Metamorph")]
        public async Task CanGetCurrencyOverviews(string league)
        {
            foreach (var currencyType in Enum.GetValues(typeof(CurrencyType)))
            {
                var actualResponse = await _sut.GetCurrencyOverview(league, (CurrencyType)currencyType);
                Assert.IsNotNull(actualResponse);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        [DataRow("InvalidLeague")]
        public async Task InvalidLeagueShouldThrowException(string league)
        {
            var actualResponse = await _sut.GetItemOverview(league, ItemType.BaseType);
        }

    }
}
