using System.Collections.Generic;
using Newtonsoft.Json;

namespace LibSharpQ.Models
{
    public class LanguageLayout
    {
        public string Language { get; set; }
        public string Location { get; set; }
        public long LanguageId { get; set; }
        public string LanguageTag { get; set; }
        public List<Zone> Zones { get; set; }
    }
}