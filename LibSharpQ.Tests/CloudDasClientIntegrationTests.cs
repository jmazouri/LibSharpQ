using System;
using System.Threading.Tasks;
using LibSharpQ.Models;
using Xunit;

namespace LibSharpQ.Tests
{
    public class CloudDasClientIntegrationTests
    {
        private CloudDasClient _client;

        private async Task<CloudDasClient> GetClient()
        {
            var client = new CloudDasClient();
            await client.Login(TestUtil.GetInstanceFromConfig<OAuthTokenRequest>());

            _client = client;

            return _client;
        }

        [Fact]
        public async Task TestGetDevices()
        {
            var result = await (await GetClient()).GetDevices();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task TestGetDeviceDefinitions()
        {
            var result = await (await GetClient()).GetDeviceDefinitions();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task TestGetPredefinedColors()
        {
            var result = await (await GetClient()).GetPredefinedColors();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task TestGetZones()
        {
            var result = await (await GetClient()).GetZones("DK5QPID");
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task TestGetEffects()
        {
            var result = await (await GetClient()).GetEffects("DK5QPID");
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task TestGetSignals()
        {
            var result = await (await GetClient()).GetSignals();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestSendSignal()
        {
            var signal = new Signal("Hey there", "KEY_H", "#00F");
            var result = await (await GetClient()).SendSignal(signal);

            Assert.True(result.Id != 0);
        }

        [Fact]
        public async Task TestDeleteSignal()
        {
            var client = await GetClient();

            var signal = new Signal("Hey there", "KEY_H", "#00F");
            var result = await client.SendSignal(signal);
            await client.DeleteSignal(result.Id);
        }

        [Fact]
        public async Task TestDeleteAllSignals()
        {
            await (await GetClient()).DeleteAllSignals();
        }
    }
}
