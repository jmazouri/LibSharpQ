using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LibSharpQ.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LibSharpQ.Models
{
    public class Signal
    {
        private static readonly Regex _locationZoneRegex = new Regex(@"(\d*),(\d*)", RegexOptions.Compiled);

        [JsonIgnoreSerialization]
        public int Id { get; set; }
        public string ClientName { get; set; }

        public string Name { get; set; }
        public string ZoneId { get; set; }
        public string Color { get; set; }
        public string Pid { get; set; } = "DK5QPID";

        public string Message { get; set; }
        public string Effect { get; set; } = "SET_COLOR";
        public string Action { get; set; }

        public bool? IsRead { get; set; }
        public bool? IsArchived { get; set; }
        public bool? IsMuted { get; set; }

        [JsonIgnoreSerialization]
        [JsonConverter(typeof(UnixDateTimeMsConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonIgnoreSerialization]
        [JsonConverter(typeof(UnixDateTimeMsConverter))]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// The x,y position of this signal, if the zone is coordinate-based. Otherwise, (-1, -1)
        /// </summary>
        [JsonIgnore]
        public (int x, int y) ZonePosition
        {
            get
            {
                var match = _locationZoneRegex.Match(ZoneId);
                if (match.Groups.Count > 0)
                {
                    return (int.Parse(match.Groups[0].Value), int.Parse(match.Groups[1].Value));
                }

                return (-1, -1);
            }
        }

        public Signal(string name, string zoneId, string color)
        {
            Name = name;
            ZoneId = zoneId;
            Color = color;
        }

        /// <summary>
        /// Create a signal with the specified parameters
        /// </summary>
        /// <param name="name">The name of the signal</param>
        /// <param name="zoneId">The zone for the signal to appear</param>
        /// <param name="color">The color for the signal. Will use the Name if not null, otherwise HexCode.</param>
        public Signal(string name, string zoneId, Color color) : this(name, zoneId, color.Name ?? color.HexCode) { }

        /// <summary>
        /// Create a signal with the specified parameters
        /// </summary>
        /// <param name="name">The name of the signal</param>
        /// <param name="position">The location for the signal to appear</param>
        /// <param name="color">The color for the signal. Will use the Name if not null, otherwise HexCode.</param>
        public Signal(string name, (int x, int y) position, Color color) : this(name, position, color.Name ?? color.HexCode) { }

        /// <summary>
        /// Create a signal with the specified parameters
        /// </summary>
        /// <param name="name">The name of the signal</param>
        /// <param name="position">The location for the signal to appear</param>
        /// <param name="color">The color for the signal. Should be a predefined color name, or hexcode.</param>
        public Signal(string name, (int x, int y) position, string color) : this(name, $"{position.x},{position.y}", color) { }

        [JsonConstructor]
        private Signal() { }
    }
}
