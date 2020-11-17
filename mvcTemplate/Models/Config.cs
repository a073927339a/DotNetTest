using System;
using Realtek.Jawapi.Core;

namespace Realtek.IntraLogin.Models
{
    public class Config
    {

        public Config() { }

        public static string GetConfigValue(String path, string key)
        {
            string value = "";
            IConfig config = ConfigFactory.Create(AppDomain.CurrentDomain.BaseDirectory + @"bin\" + path + ".config");
            value = config.GetValue(key);
            return value;
        }
    }
}