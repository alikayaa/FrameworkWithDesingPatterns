using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DAI.Configuration
{
    public class ConfigManager
    {
        private IConfigReader reader=null;
        private static ConfigManager configManager = null;
        private static object lockObject = new object();

        private ConfigManager()
        {
            bool tddMode = bool.Parse(ConfigurationManager.AppSettings["TDDMode"].ToString());
            if (tddMode)
                reader = new TestConfigurationReader();
            else
                reader = new ConfigurationReader();
        }

        public static ConfigManager Instance()
        {
            if (configManager == null)
            {
                lock (lockObject)
                {
                    if (configManager == null)
                        configManager = new ConfigManager();
                }
            }
            return configManager;
        }

        public T GetValue<T>(string name)
        {
            string value=reader.ReadKey(name);
            T result = (T)Convert.ChangeType(value, typeof(T));
            return result;
        }

        public string GetConnectionString(string name)
        {
            string result = reader.ReadConncetionString(name);
            return result;
        }


    }
}
