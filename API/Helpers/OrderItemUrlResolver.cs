using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Options;

namespace API.Helpers
{
    public class OrderItemUrlResolver: IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly ApiUrl _apiUrl;
        public OrderItemUrlResolver(IOptions<ApiUrl> apiUrl)
        {
            _apiUrl = apiUrl.Value;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string? destMember, ResolutionContext context)
        {
            return _apiUrl.BaseUrl + source.ItemOrdered.PhotoUrl;
        }
    }
}