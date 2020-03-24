using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using testApi.Controllers;
using testApi.Utils;
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

        [Theory]
        [InlineData(20)]
        [InlineData(30)]
        public async void TestPost(int age)
        {
            var person = new Person() { Name = "wwmin", Age = age };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(person));
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var data = await httpClient.PostAsync("/api/values", httpContent);
            var result = await data.Content.ReadAsStringAsync();
            Assert.Equal(age.ToString(), result);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(5000, 6000)]
        public void TestAdd(int i, int j)
        {
            var sum = Util.Add(i, j);
            Assert.Equal(i + j, sum);
        }
    }
}
