using EmailAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;

namespace EmailAPITests {
    [TestClass]
    public class EmailTests {
        private readonly HttpClient _client;

        public EmailTests() {
            var application = new CustomWebApplicationFactory<Program>();
            _client = application.CreateClient();
        }

        [TestMethod]
        public async Task ServerKeys_GetEndpointWithNoSubcriptionKey_FailsAuthentication() {
            // Arrange
            const string ENDPOINT = "/servers/keys";

            // Act
            HttpResponseMessage response = await _client.GetAsync(ENDPOINT);

            // Assert
            HttpStatusCode code = response.StatusCode;
            Assert.AreEqual(code, HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public async Task ServerKeys_GetEndpointWithSubscriptionKey_ReturnsCollection() {
            // Arrange
            const string ENDPOINT = "/servers/keys";
            _client.DefaultRequestHeaders.Add("Subscription-Key", "key");

            // Act
            HttpResponseMessage response = await _client.GetAsync(ENDPOINT);

            // Assert
            HttpStatusCode code = response.StatusCode;
            Assert.AreEqual(code, HttpStatusCode.OK);

            string responseBody = await response.Content.ReadAsStringAsync();
            var keys = JsonSerializer.Deserialize<List<string>>(responseBody);

            Assert.IsNotNull(keys);
        }
    }

    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class {
        protected override IHost CreateHost(IHostBuilder builder) {
            builder.ConfigureAppConfiguration((context, config) => {
                config.AddInMemoryCollection(new Dictionary<string, string?> {
                    { "EmailAPI:Settings:Token", "key" }
                });
            });

            return base.CreateHost(builder);
        }
    }
}