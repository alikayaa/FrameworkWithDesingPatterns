using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DAI.Configuration
{
    internal class ConfigurationReader:IConfigReader
    {
        public string ReadKey(string name)
        {
            string value = ConfigurationManager.AppSettings[name].ToString();
            return value;
        }


        public string ReadConncetionString(string name)
        {
            string connStr = ConfigurationManager.ConnectionStrings[name].ToString();
            return connStr;
        }
    }
}
