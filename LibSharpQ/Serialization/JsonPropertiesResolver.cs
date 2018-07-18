using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace LibSharpQ.Serialization
{
    public class JsonPropertiesResolver : DefaultContractResolver
    {
        public JsonPropertiesResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy();
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            //Ignore properties that have the JsonIgnoreSerialization attribute
            return base.GetSerializableMembers(objectType)
                             .Where(pi => !Attribute.IsDefined(pi, typeof(JsonIgnoreSerializationAttribute)))
                             .ToList();
        }
    }
}
