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
        public CloudDasClient(HttpClient client, string baseUrl = "https://q.daskeyboard.com/") : base(client, baseUrl) { }

        /// <summary>
        /// Perform an asynchronous OAuth login request with the given credentials
        /// </summary>
        /// <param name="credentials">The credentials to authenticate with</param>
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

            //Set the auth header on our client so future request are authenticated
            var result = await _client.GetResult<OAuthTokenResponse>(msg).ConfigureAwait(false);
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
            return await _client.GetResult<T>(msg).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Retrieve the list of devices on the logged-in account
        /// </summary>
        public Task<IReadOnlyList<Device>> GetDevices()
            => AuthenticatedRequest<IReadOnlyList<Device>>("api/1.0/devices");

        /// <summary>
        /// Retrieve all device definitions from the API
        /// </summary>
        public Task<IReadOnlyList<DeviceDefinition>> GetDeviceDefinitions()
            => AuthenticatedRequest<IReadOnlyList<DeviceDefinition>>("api/1.0/device_definitions");

        /// <summary>
        /// Retrieve the predefined, named colors from the API
        /// </summary>
        public Task<IReadOnlyList<Color>> GetPredefinedColors()
            => AuthenticatedRequest<IReadOnlyList<Color>>("api/1.0/colors");

        /// <summary>
        /// Retrieve all the valid zones for the given device PID
        /// </summary>
        /// <param name="pid">The device PID to get zones for. Defaults to D5QPID</param>
        public Task<IReadOnlyList<Zone>> GetZones(string pid = "DK5QPID")
            => AuthenticatedRequest<IReadOnlyList<Zone>>($"api/1.0/{pid}/zones");

        /// <summary>
        /// Retrieve all valid effects for the given device PID
        /// </summary>
        /// <param name="pid">The device PID to get effects for. Defaults to D5QPID</param>
        public Task<IReadOnlyList<Effect>> GetEffects(string pid = "DK5QPID")
            => AuthenticatedRequest<IReadOnlyList<Effect>>($"api/1.0/{pid}/effects");

        /// <summary>
        /// Retrieve all signals from the API
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
                var result = await _client.GetResult<PaginatedSignalResponse>(msg).ConfigureAwait(false);

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
