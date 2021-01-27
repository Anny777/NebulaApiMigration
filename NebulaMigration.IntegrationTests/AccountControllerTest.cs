namespace NebulaMigration.IntegrationTests
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Models;
    using Polly;
    using Xunit;

    public class AccountControllerTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task AuthenticateShouldReturnOkStatusCodeAndToken()
        {
            var r = await this.Authenticate();
            Assert.NotEmpty(r.Access_token);
            Assert.NotEmpty(r.Token_type);
            Assert.NotEmpty(r.Username);
        }

        internal async Task<AuthenticateResponse> Authenticate()
        {
            var requestDto = new { Username = "admin@nebula.com", Password = "Zxcvbnm,./1" };
            var content =
                new StringContent(JsonSerializer.Serialize(requestDto), Encoding.UTF8, "application/json");
            using var response = await Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(2 * retryAttempt))
                .ExecuteAsync(() => this.httpClient.PostAsync($"{Environments.Host}/api/Account", content))
                .ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var r = JsonSerializer.Deserialize<AuthenticateResponse>(body);
            return r;
        }
    }
}