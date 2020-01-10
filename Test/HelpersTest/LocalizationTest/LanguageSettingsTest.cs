using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sidekick.Helpers.POETradeAPI;
using Sidekick.Helpers.Localization;

namespace Test.HelpersTest.LocalizationTest
{
    [TestClass]
    public class LanguageSettingsTest
    {
        [TestInitialize]
        public void Initialize()
        {
            var tradeClientMock = new Mock<ITradeClient>(MockBehavior.Strict);
            tradeClientMock.Setup(tradeClient => tradeClient.FetchAPIData()).Returns(Task.CompletedTask);
            TradeClient.Client = tradeClientMock.Object;
        }

        //TODO add test cases
        [DataTestMethod]
        [DataRow("", Language.English)]
        public void DetectLanguage_WithInput_IsExpected(string input, Language expected)
        {
            LanguageSettings.DetectLanguage(input);
            Assert.AreEqual(expected, LanguageSettings.CurrentLanguage);
        }
    }
}
