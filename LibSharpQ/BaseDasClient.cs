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

        private bool _shouldDispose = false;
        private bool _disposed = false;

        protected BaseDasClient(HttpClient client, string baseUrl)
        {
            BaseUrl = baseUrl;
            _client = client;
        }

        protected BaseDasClient(string baseUrl) : this(new HttpClient { BaseAddress = new Uri(baseUrl) }, baseUrl)
        {
            _shouldDispose = true;
        }

        public async Task DeleteSignal(int signalId)
        {
            var msg = new HttpRequestMessage(HttpMethod.Delete, $"api/1.0/signals/{signalId}");
            await _client.SendAsync(msg).ConfigureAwait(false);
        }

        public Task DeleteSignal(Signal signal)
        {
            return DeleteSignal(signal.Id);
        }

        public abstract Task<IReadOnlyList<Signal>> GetSignals(bool retrieveAll = true);

        public virtual async Task<Signal> SendSignal(Signal signal)
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

            return await _client.GetResult<Signal>(msg).ConfigureAwait(false);
        }

        public virtual async Task DeleteSignals(IEnumerable<int> signalIds)
        {
            await Task.WhenAll(signalIds.Select(id => DeleteSignal(id))).ConfigureAwait(false);
        }

        public virtual Task DeleteSignals(IEnumerable<Signal> signals)
        {
            return DeleteSignals(signals.Select(d=>d.Id));
        }

        public virtual async Task DeleteAllSignals()
        {
            var signals = await GetSignals().ConfigureAwait(false);
            await DeleteSignals(signals).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing && _shouldDispose)
            {
                _client.Dispose();
            }

            _disposed = true;
        }

        ~BaseDasClient()
        {
            Dispose(false);
        }
    }
}
