using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibSharpQ.Tests
{
    public class CloudDasClientUnitTests
    {
        [Fact]
        public async Task TestThrowsExceptionIfNotLoggedIn()
        {
            var client = new CloudDasClient();
            await Assert.ThrowsAsync<InvalidOperationException>(() => client.GetDevices());
        }
    }
}
