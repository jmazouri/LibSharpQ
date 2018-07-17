using System;
using System.Collections.Generic;
using System.Text;
using LibSharpQ.Serialization;
using Newtonsoft.Json;

namespace LibSharpQ.Models
{
    public class Device
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Pid { get; set; }
        public string FirmwareVersion { get; set; }
        public string Vid { get; set; }
        public string Description { get; set; }
        public string Uuid { get; set; }
        public long VersionNumber { get; set; }
        public string UpdateOrigin { get; set; }
        public bool IsDeleted { get; set; }

        [JsonConverter(typeof(UnixDateTimeMsConverter))]
        public DateTime CreatedAt { get; set; }
        
        [JsonConverter(typeof(UnixDateTimeMsConverter))]
        public DateTime UpdatedAt { get; set; }
    }
}
