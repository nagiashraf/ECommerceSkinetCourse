using API.DTOs;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Options;

namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string?>
    {
        private readonly ApiUrl _apiUrl;
        public ProductUrlResolver(IOptions<ApiUrl> apiUrl)
        {
            _apiUrl = apiUrl.Value;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string? destMember, ResolutionContext context)
        {
            return _apiUrl.BaseUrl + source.PhotoUrl;
        }
    }
}