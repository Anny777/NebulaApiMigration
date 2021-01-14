namespace NebulaMigration.IntegrationTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Polly;
    using Xunit;
    using Xunit.Sdk;

    public class AccountControllerTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task AuthenticateShouldReturnOkStatusCodeAndToken()
        {
            var requestDto = new { Username = "admin@nebula.com", Password = "Zxcvbnm,./1" };
            var content =
                new StringContent(JsonSerializer.Serialize(requestDto), Encoding.UTF8, "application/json");
            using var response = await Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(2 * retryAttempt))
                .ExecuteAsync(() => httpClient.PostAsync("http://web-api/api/Account", content))
                .ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}