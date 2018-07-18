using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LibSharpQ
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetResult<T>(this HttpClient client, HttpRequestMessage msg)
        {
            var response = await client.SendAsync(msg).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            using (response)
            {
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
        }
    }
}
