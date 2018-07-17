using System.Collections.Generic;
using Newtonsoft.Json;

namespace LibSharpQ.Models
{
    public class Zone
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<long> LedIds { get; set; }
    }
}