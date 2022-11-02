namespace API.Tests.ControllersTests
{
    public class ProductControllerTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        public ProductControllerTests(TestingWebAppFactory<Program> factory) => _client = factory.CreateClient();

        [Fact]
        public async Task GetProducts_ReturnsAndMappedSuccessfully()
        {
            var response = await _client.GetAsync("/api/v1/products");
            var responseString = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.DoesNotContain("null", responseString);
            Assert.DoesNotContain("ProductTypeId", responseString);
            Assert.DoesNotContain("ProductBrandId", responseString);
            Assert.Contains("https://localhost:7159/images/products", responseString);
        }

        [Fact]
        public async Task GetProduct_ReturnsAndMappedSuccessfully()
        {
            var response = await _client.GetAsync("/api/v1/products/1");
            var responseString = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.DoesNotContain("null", responseString);
            Assert.DoesNotContain("ProductTypeId", responseString);
            Assert.DoesNotContain("ProductBrandId", responseString);
            Assert.Contains("https://localhost:7159/images/products", responseString);
        }

        [Fact]
        public async Task GetProduct_NotFound()
        {
            var response = await _client.GetAsync("/api/v1/products/-1");

            var expectedResponseStatusCode = 404;
            var actualResponseStatusCode = Convert.ToInt32(response.StatusCode);

            Assert.Equal(expectedResponseStatusCode, actualResponseStatusCode);
        }

        [Fact]
        public async Task GetTypes_ReturnsSuccessfullyAndNotEmpty()
        {
            var response = await _client.GetAsync("/api/v1/products/types");
            var responseString = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("name", responseString);
        }

        [Fact]
        public async Task GetBrands_ReturnsSuccessfullyAndNotEmpty()
        {
            var response = await _client.GetAsync("/api/v1/products/brands");
            var responseString = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("name", responseString);
        }
    }
}