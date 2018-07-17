using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LibSharpQ.Models
{
    public class DeviceDefinition
    {
        public string Name { get; set; }
        public string Vid { get; set; }
        public string Pid { get; set; }
        public string ModelNumber { get; set; }
        public string Description { get; set; }
        public LanguageLayout LanguageLayout { get; set; }
    }
}
