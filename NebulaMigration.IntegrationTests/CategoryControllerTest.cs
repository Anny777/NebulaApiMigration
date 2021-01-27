namespace NebulaMigration.IntegrationTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Commands;
    using Models.Enums;
    using Polly;
    using Xunit;

    public class CategoryControllerTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task PostShouldReturn201Response()
        {
            var newCategoryToBar = new CreateCategoryCommand
            {
                Code = "Код в бар",
                Name = "Наименование в бар",
                ExternalId = 12345,
                WorkshopType = WorkshopType.Bar,
            };
            
            var newCategoryToKitchen = new CreateCategoryCommand
            {
                Code = "Код на кухню",
                Name = "Наименование на кухню",
                ExternalId = 123456,
                WorkshopType = WorkshopType.Kitchen,
            };
            
            var token = await new AccountControllerTest().Authenticate().ConfigureAwait(false);
            this.httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.Token_type} {token.Access_token}");
            
            var response = await this.Create(newCategoryToBar);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var response2 = await this.Create(newCategoryToKitchen);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        }

        private async Task<HttpResponseMessage> Create(CreateCategoryCommand newCategoryToBar)
        {
            var content =
                new StringContent(JsonSerializer.Serialize(newCategoryToBar), Encoding.UTF8, "application/json");
            using var response = await Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(2 * retryAttempt))
                .ExecuteAsync(() => this.httpClient.PostAsync($"{Environments.Host}/api/Category", content))
                .ConfigureAwait(false);
            return response;
        }
    }
}