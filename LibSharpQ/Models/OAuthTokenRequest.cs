using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LibSharpQ.Models
{
    public class OAuthTokenRequest
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; } = "client_credentials";
    }
}
