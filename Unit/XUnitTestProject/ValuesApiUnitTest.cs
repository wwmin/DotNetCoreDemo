using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using Xunit;

namespace XUnitTestProject
{
    public class ValuesApiUnitTest
    {

        private TestServer testServer;
        private HttpClient httpClient;

        public ValuesApiUnitTest()
        {
            testServer = new TestServer(new WebHostBuilder().UseStartup<testApi.Startup>());
            httpClient = testServer.CreateClient();
        }

        [Fact]
        public async void GetTest()
        {
            var data = await httpClient.GetAsync("/api/values/100");
            var result = await data.Content.ReadAsStringAsync();

            Assert.Equal("200", result);
        }
    }
}
