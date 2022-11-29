using System.Linq.Expressions;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class SortedOrdersWithItemsAndDeliverySpecification : BaseSpecifcation<Order>
    {
        public SortedOrdersWithItemsAndDeliverySpecification(string buyerEmail)
            : base(o => o.BuyerEmail == buyerEmail)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }

        public SortedOrdersWithItemsAndDeliverySpecification(int id, string buyerEmail)
            : base(o => o.Id == id && o.BuyerEmail == buyerEmail)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}