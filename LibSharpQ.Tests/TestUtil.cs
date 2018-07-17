using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace LibSharpQ.Tests
{
    public static class TestUtil
    {
        private static JObject _rootConfig;
        private static JObject RootConfig
        {
            get
            {
                _rootConfig = JObject.Parse(File.ReadAllText("config.json"));
                return _rootConfig;
            }
        }

        public static T GetInstanceFromConfig<T>()
        {
            string name = typeof(T).Name;
            return RootConfig.GetValue(name).ToObject<T>();
        }
    }
}
