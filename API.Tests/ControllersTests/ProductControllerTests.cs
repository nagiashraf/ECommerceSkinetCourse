using System.Text.Json;
using API.DTOs;
using API.Helpers;

namespace API.Tests.ControllersTests
{
    public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public ProductControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_ReturnsAndMapsSuccessfully()
        {
            var response = await _client.GetAsync("/api/v1/products");
            var responseString = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.DoesNotContain("null", responseString);
            Assert.DoesNotContain("ProductTypeId", responseString);
            Assert.DoesNotContain("ProductBrandId", responseString);
            Assert.Contains("https://", responseString);
        }

        [Theory]
        [InlineData("")]
        [InlineData("?sort=foo")]
        [InlineData("?sort=priceAsc")]
        [InlineData("?sort=priceDsc")]

        public async Task GetProducts_SortsSuccessfully(string? queryString)
        {
            var response = await _client.GetAsync("/api/v1/products" + queryString);
            string responseString = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Pagination<ProductToReturnDto>? deserializedProducts = JsonSerializer.Deserialize<Pagination<ProductToReturnDto>>(responseString, jsonOptions);
            var testResult = true;

            if (deserializedProducts is not null)
            {
                switch (queryString)
                {
                    case "?sort=priceAsc":
                        for (int i = 1; i < 10; i++)
                        {
                            if (deserializedProducts.Data.ElementAt(i).Price < deserializedProducts.Data.ElementAt(i - 1).Price)
                            {
                                testResult = false;
                                break;
                            }
                        }
                        break;
                    case "?sort=priceDsc":
                        for (int i = 1; i < 10; i++)
                        {
                            if (deserializedProducts.Data.ElementAt(i).Price > deserializedProducts.Data.ElementAt(i - 1).Price)
                            {
                                testResult = false;
                                break;
                            }
                        }
                        break;
                    default:
                        for (int i = 1; i < 10; i++)
                        {
                            if (deserializedProducts.Data.ElementAt(i).Name![0] < deserializedProducts.Data.ElementAt(i - 1).Name![0])
                            {
                                testResult = false;
                                break;
                            }
                        }
                        break;
                }
            }
            else
            {
                testResult = false;
            }

            Assert.True(testResult);
        }

        [Theory]
        [InlineData("?typeId=1", "productType\":\"Boards")]
        [InlineData("?brandId=1", "productBrand\":\"NIKE")]
        [InlineData("?brandId=1&typeId=1", "productType\":\"Boards\",\"productBrand\":\"NIKE")]
        public async Task GetProducts_FiltersSuccessfully(string? queryString, string expected)
        {
            var response = await _client.GetAsync("/api/v1/products" + queryString);
            string responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains(expected, responseString);
        }

        [Theory]
        [InlineData("", "pageIndex\":1,\"pageSize\":10,\"count\":18")]
        [InlineData("?pageIndex=2&pageSize=5", "pageIndex\":2,\"pageSize\":5,\"count\":18")]
        [InlineData("?brandId=2&typeId=3", "pageIndex\":1,\"pageSize\":10,\"count\":2")]

        public async Task GetProducts_PaginatesSuccessfully(string? queryString, string expected)
        {
            var response = await _client.GetAsync("/api/v1/products" + queryString);
            string responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains(expected, responseString);
        }

        [Theory]
        [InlineData("?search=purple", "Purple")]
        public async Task GetProducts_SearchesSuccessfully(string? queryString, string expected)
        {
            var response = await _client.GetAsync("/api/v1/products" + queryString);
            string responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains(expected, responseString);
        }

        [Fact]
        public async Task GetProduct_ReturnsAndMapsSuccessfully()
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