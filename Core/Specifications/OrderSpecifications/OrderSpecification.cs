using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Core.Extensions;
using Core.Interfaces;

namespace Core.Specifications.OrderSpecifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string email)
            : base(o => o.BuyerEmail == email)
        {
            AddInclude(x => x.OrderItems,
                x => x.DeliveryMethod!);
            AddOrderByDescending(x => x.OrderDate);
        }

        public OrderSpecification(string email, int orderId)
            : base(o => o.BuyerEmail == email && o.Id == orderId)
        {
            AddInclude($"{nameof(Order.OrderItems)}", $"{nameof(Order.DeliveryMethod)}");  // you can use it with thenInclude by the write the prop.nestedProp
        }

        public OrderSpecification(OrderSpecParams specParams)
            : base(o => string.IsNullOrWhiteSpace(specParams.Filter)
            || o.Status == ParseStatus(specParams.Filter))
        {
            AddPagination(specParams.PageIndex, specParams.PageSize);

            AddInclude(x => x.OrderItems,
               x => x.DeliveryMethod!);

            AddOrderByDescending(x => x.OrderDate);
        }

        public OrderSpecification(int id)
           : base(o => o.Id == id)
        {
            AddInclude($"{nameof(Order.OrderItems)}", $"{nameof(Order.DeliveryMethod)}");
        }

        static OrderStatus? ParseStatus(string status)
        {
            if (Enum.TryParse<OrderStatus>(status?.Trim(), true, out var result)) return result;

            return null;
        }
    }


}
