using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAI.Configuration
{
    internal class TestConfigurationReader:IConfigReader
    {
        private string _LogSource = "SalesSystem.LogLayer,SalesSystem.LogLayer.TraceLogger";
        private string _LogFormatter = "SalesSystem.LogLayer,SalesSystem.LogLayer.HtmlLogFormatter";

        private string _ExceptionFormatter = "SalesSystem.ExceptionLayer.ExceptionFormatter,SalesSystem.ExceptionLayer.ExceptionFormatter.HtmlExceptionFormatter";
        public string ReadKey(string name)
        {
            string result = "";
            switch (name)
            {
                case "LogSource":
                    result = _LogSource;
                    break;
                case "LogFormatter":
                    result = _LogFormatter;
                    break;
                case "ExceptionFormatter":
                    result = _ExceptionFormatter;
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        private string _connectionStringTest = "";

        public string ReadConncetionString(string name)
        {
            return _connectionStringTest;
        }
    }
}
