using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sidekick.Helpers;

namespace Test.HelpersTest
{
    //TODO add tests
    [TestClass]
    public class LoggerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Logger.Logs.Clear();
        }

        [TestMethod]
        public void Log_EmptyString_NotLogged()
        {
            // arrange
            var text = "";

            // act
            Logger.Log(text);

            // assert
            Assert.IsFalse(Logger.Logs.Any());
        }

        [TestMethod]
        public void Log_TextIsNull_NotLogged()
        {
            // arrange
            string text = null;

            // act
            Logger.Log(text);

            // assert
            Assert.IsFalse(Logger.Logs.Any());
        }

        [TestMethod]
        public void Log_SomeText_IsInLogs()
        {
            // arrange
            var text = "haha";

            // act
            Logger.Log(text);

            // assert
            Assert.IsNotNull(Logger.Logs.FirstOrDefault(log => log.Message.Equals(text)));
        }

        [TestMethod]
        public void Log_LogStateWarning_IsInLogs()
        {
            // arrange
            var state = LogState.Warning;

            // act
            Logger.Log("haha", state);

            // assert
            Assert.IsNotNull(Logger.Logs.FirstOrDefault(log => log.State == state));
        }

        [TestMethod]
        public void Log_1000Items_100InLogs()
        {
            // arrange 
            var expected = 100;

            // act
            for (var i = 0; i < 1000; i++)
            {
                Logger.Log("haha");
            }

            // assert
            Assert.AreEqual(expected, Logger.Logs.Count);
        }

        [TestMethod]
        public void Log_SomeText_RaisesMessageLogged()
        {
            // arrange
            var timesMethodCalled = 0;
            Logger.MessageLogged += (_, __) => timesMethodCalled++;

            // act
            Logger.Log("haha");

            // assert
            Assert.AreEqual(1, timesMethodCalled);
        }

        [TestMethod]
        public void LogException_ExceptionNull_NotLogged()
        {
            // arrange
            Exception exception = null;

            // act
            Logger.LogException("haha", exception);

            // assert
            Assert.IsFalse(Logger.Logs.Any());
        }

        [TestMethod]
        public void LogException_SomeExceptionInformation_IsLogged()
        {
            // arrange
            var message = "haha";
            Exception exception;

            try
            {
                throw new Exception(message);
            }
            catch (Exception exc)
            {
                exception = exc;
            }

            // act
            Logger.LogException("", exception);

            // assert
            Assert.IsNotNull(Logger.Logs.FirstOrDefault(log => log.Message.Contains(message) && log.Message.Contains(exception.StackTrace)));
        }

        [TestMethod]
        public void Clear_NoItems_StillNoItems()
        {
            // act
            Logger.Clear();

            // assert
            Assert.IsFalse(Logger.Logs.Any());
        }

        [TestMethod]
        public void Clear_OneItem_IsRemoved()
        {
            // arrange
            Logger.Log("haha");

            // act
            Logger.Clear();

            // assert
            Assert.IsFalse(Logger.Logs.Any());
        }
    }
}
