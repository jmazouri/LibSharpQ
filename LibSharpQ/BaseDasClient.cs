using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibSharpQ.Models;
using LibSharpQ.Serialization;
using Newtonsoft.Json;

namespace LibSharpQ
{
    public abstract class BaseDasClient : IDasClient, IDisposable
    {
        public string BaseUrl { get; protected set; }
        protected HttpClient _client;

        public BaseDasClient(string baseUrl)
        {
            BaseUrl = baseUrl;

            _client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task DeleteSignal(int signalId)
        {
            var msg = new HttpRequestMessage(HttpMethod.Delete, $"api/1.0/signals/{signalId}");
            await _client.SendAsync(msg);
        }

        public abstract Task<IReadOnlyList<Signal>> GetSignals(bool retrieveAll = true);

        public async Task<Signal> SendSignal(Signal signal)
        {
            string serializedSignal = JsonConvert.SerializeObject(signal, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new JsonPropertiesResolver()
            });

            var msg = new HttpRequestMessage(HttpMethod.Post, "api/1.0/signals")
            {
                Content = new StringContent(serializedSignal, Encoding.UTF8, "application/json")
            };

            return await _client.GetResult<Signal>(msg);
        }

        public async Task DeleteSignals(IEnumerable<int> signalIds)
        {
            foreach (var id in signalIds)
            {
                await DeleteSignal(id);
            }
        }

        public Task DeleteSignals(IEnumerable<Signal> signals)
        {
            return DeleteSignals(signals);
        }

        public async Task DeleteAllSignals()
        {
            var signals = await GetSignals();
            await DeleteSignals(signals);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
