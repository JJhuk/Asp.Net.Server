using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Server;
using Tests.Helpers;
using Xunit;

namespace Tests.Integration
{
    public class MockingIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private static readonly Dictionary<string, string> DefaultRequest;
        private readonly HttpClient _testClient;

        static MockingIntegrationTest()
        {
            DefaultRequest = new Dictionary<string, string>
            {
                {"Username", "hello"},
                {"Email", "email@eamil.com"},
                {"Password", "password"}
            };
        }

        public MockingIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            _testClient = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "Test", options => { });
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        }

        [Fact]
        public async Task Scenario_1()
        {
            //given
            //when
            var regRes = await RegisterAsync();
            var loginRes = await _testClient.GetAsync("Users/1");

            //then
            regRes.StatusCode.Should().Be(HttpStatusCode.OK);
            loginRes.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Scenario_2()
        {
            //given
            var changeAccount = new Dictionary<string, string>
            {
                {"Username", "bye"},
                {"Email", "email@email.com"},
                {"Password", "password"}
            };

            //when
            await RegisterAsync();

            var beforeChangeAccountAuth = await AuthenticateAsync(DefaultRequest);

            var getResponseMessage = await _testClient.GetAsync("Users/1");
            var putResponseMessage = await _testClient.PutAsync("Users/1", new JsonContent(changeAccount));

            var successAuth = await AuthenticateAsync(changeAccount);
            var invalidAuth = await AuthenticateAsync(DefaultRequest);

            //then
            beforeChangeAccountAuth.StatusCode.Should().Be(StatusCodes.Status200OK);
            getResponseMessage.StatusCode.Should().Be(StatusCodes.Status200OK);
            putResponseMessage.StatusCode.Should().Be(StatusCodes.Status200OK);
            successAuth.StatusCode.Should().Be(StatusCodes.Status200OK);
            invalidAuth.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        private async Task<HttpResponseMessage> AuthenticateAsync(Dictionary<string, string> defaultRequest)
        {
            return await _testClient.PostAsync("Users/authenticate", new JsonContent(defaultRequest));
        }

        private async Task<HttpResponseMessage> RegisterAsync(Dictionary<string, string> defaultRequest = default)
        {
            defaultRequest ??= DefaultRequest;
            return await _testClient.PostAsync("Users/register", new JsonContent(defaultRequest));
        }
    }

    public class JsonContent : StringContent
    {
        public JsonContent(object content) : base(JsonConvert.SerializeObject(content), Encoding.UTF8,
            "application/json")
        {
        }
    }
}