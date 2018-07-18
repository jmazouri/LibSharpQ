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
        public LocalDasClient(HttpClient client, string baseUrl = "http://localhost:27301/") : base(client, baseUrl) { }

        /// <summary>
        /// Retrieve all signals from the API
        /// </summary>
        /// <param name="retrieveAll">Whether to retrieve all pages of results, or just the first</param>
        /// <returns>A collection of signals that were previously sent</returns>
        public override async Task<IReadOnlyList<Signal>> GetSignals(bool retrieveAll = true)
        {
            var msg = new HttpRequestMessage(HttpMethod.Get, "api/1.0/signals");
            return await _client.GetResult<List<Signal>>(msg).ConfigureAwait(false);
        }

        public override async Task DeleteSignals(IEnumerable<int> signalIds)
        {
            //Can't delete them in paralell because the 5Q desktop software crashes...
            foreach (var id in signalIds)
            {
                await DeleteSignal(id).ConfigureAwait(false);
            }
        }
    }
}
