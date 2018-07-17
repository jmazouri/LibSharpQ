using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LibSharpQ.Models;
using LibSharpQ.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LibSharpQ
{
    public class CloudDasClient : BaseDasClient
    {
        public CloudDasClient(string baseUrl = "https://q.daskeyboard.com/") : base(baseUrl) { }
        
        public async Task Login(OAuthTokenRequest credentials)
        {
            if (String.IsNullOrWhiteSpace(credentials.ClientId) || String.IsNullOrWhiteSpace(credentials.ClientSecret))
            {
                throw new ArgumentException("ClientId and ClientSecret must be populated. Get them at https://q.daskeyboard.com/account");
            }

            var msg = new HttpRequestMessage(HttpMethod.Post, "oauth/1.4/token")
            {
                Content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json")
            };

            var result = await _client.GetResult<OAuthTokenResponse>(msg);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        }

        private void EnsureLoggedIn()
        {
            if (_client.DefaultRequestHeaders.Authorization == null)
            {
                throw new InvalidOperationException("You must call Login() before accessing this endpoint.");
            }
        }

        public async Task<T> AuthenticatedRequest<T>(string endpoint)
        {
            EnsureLoggedIn();

            var msg = new HttpRequestMessage(HttpMethod.Get, endpoint);
            return await _client.GetResult<T>(msg);
        }
        
        public Task<IReadOnlyList<Device>> GetDevices()
            => AuthenticatedRequest<IReadOnlyList<Device>>("api/1.0/devices");

        public Task<IReadOnlyList<DeviceDefinition>> GetDeviceDefinitions()
            => AuthenticatedRequest<IReadOnlyList<DeviceDefinition>>("api/1.0/device_definitions");

        public Task<IReadOnlyList<Color>> GetPredefinedColors()
            => AuthenticatedRequest<IReadOnlyList<Color>>("api/1.0/colors");

        public Task<IReadOnlyList<Zone>> GetZones(string pid = "DK5QPID")
            => AuthenticatedRequest<IReadOnlyList<Zone>>($"api/1.0/{pid}/zones");

        public Task<IReadOnlyList<Effect>> GetEffects(string pid = "DK5QPID")
            => AuthenticatedRequest<IReadOnlyList<Effect>>($"api/1.0/{pid}/effects");

        /// <summary>
        /// Retrieve all signals from the cloud API
        /// </summary>
        /// <param name="retrieveAll">Whether to retrieve all pages of results, or just the first</param>
        /// <returns>A collection of signals that were previously sent</returns>
        public override async Task<IReadOnlyList<Signal>> GetSignals(bool retrieveAll = true)
        {
            List<Signal> ret = new List<Signal>();
            int page = 0;
            bool hasMorePages = true;

            while (hasMorePages)
            {
                var msg = new HttpRequestMessage(HttpMethod.Get, $"api/1.0/signals?page={page}");
                var result = await _client.GetResult<PaginatedSignalResponse>(msg);

                ret.AddRange(result.Signals);

                if (!retrieveAll)
                {
                    break;
                }

                hasMorePages = result.HasNextPage;
                page++;
            }

            return ret;
        }
    }
}
