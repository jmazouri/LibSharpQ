using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LibSharpQ.Models
{
    public class PaginatedSignalResponse
    {
        [JsonProperty("content")]
        public List<Signal> Signals { get; set; }

        public long Size { get; set; }
        public string Sort { get; set; }
        public bool HasNextPage { get; set; }
        public long Page { get; set; }
        public long TotalElements { get; set; }
        public long TotalPages { get; set; }
    }
}
