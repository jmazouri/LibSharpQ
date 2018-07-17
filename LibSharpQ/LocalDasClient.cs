using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibSharpQ.Models;

namespace LibSharpQ
{
    public class LocalDasClient : BaseDasClient
    {
        public LocalDasClient(string baseUrl = "http://localhost:27301/") : base(baseUrl) { }

        public override async Task<IReadOnlyList<Signal>> GetSignals(bool retrieveAll = true)
        {
            var msg = new HttpRequestMessage(HttpMethod.Get, "api/1.0/signals");
            return await _client.GetResult<List<Signal>>(msg);
        }
    }
}
