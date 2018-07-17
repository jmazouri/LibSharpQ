using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LibSharpQ.Models;
using Xunit;

namespace LibSharpQ.Tests
{
    public class LocalDasClientIntegrationTests
    {
        private LocalDasClient _client = new LocalDasClient();

        [Fact]
        public async Task TestGetSignals()
        {
            var result = await _client.GetSignals();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestSendSignal()
        {
            var signal = new Signal("Hey there", "KEY_H", "#00F");
            var result = await _client.SendSignal(signal);

            Assert.True(result.Id != 0);
        }

        [Fact]
        public async Task TestDeleteSignal()
        {
            var signal = new Signal("Hey there", "KEY_H", "#00F");
            var result = await _client.SendSignal(signal);
            await _client.DeleteSignal(result.Id);
        }

        [Fact]
        public async Task TestDeleteAllSignals()
        {
            await _client.DeleteAllSignals();
        }
    }
}
