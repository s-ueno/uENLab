using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN
{
    public static class BizUtils
    {
        public static T AppSettings<T>(string key, T defaultValue = default(T))
        {
            var result = defaultValue;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var value = appSettings.Get(key);
                if (!string.IsNullOrEmpty(value))
                {
                    result = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
                }
            }
            catch
            {
                //error free
            }
            return result;
        }
    }
}
