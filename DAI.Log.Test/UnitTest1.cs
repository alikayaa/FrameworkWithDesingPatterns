using System;
using DAI.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAI.Log.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConfigReaderlarDoğruCalisiyorMu()
        {
            TestConfigReader reader = new TestConfigReader("DAI.Log,DAI.Log.TraceLogger", "DAI.Log,DAI.Log.HtmlLogFormatter");
            string logSourceValue = reader.ReadKey("LogSource");
            string logFormatterValue = reader.ReadKey("LogFormatter");

            Assert.AreEqual(logSourceValue, "DAI.Log,DAI.Log.TraceLogger");
            Assert.AreEqual(logFormatterValue, "DAI.Log,DAI.Log.HtmlLogFormatter");
        }

        [TestMethod]
        public void LoglamaDogruCalisiyorMu()
        {
            LogManager manager = LogManager.CreateInstance(new TestConfigReader("DAI.Log,DAI.Log.TraceLogger", "DAI.Log,DAI.Log.HtmlLogFormatter"));
            Log myLog = new Log("Deneme mesaj", LogPriority.Normal, DateTime.Now);
            bool result = manager.WriteLog(myLog);
            Assert.AreEqual(result, true);
        }
    }
}
