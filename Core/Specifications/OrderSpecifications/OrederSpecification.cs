using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Specifications.OrderSpecifications
{
    public class OrederSpecification : BaseSpecification<Order>
    {
        public OrederSpecification(string email)
            : base(o => o.BuyerEmail == email)
        {
            AddInclude(x => x.OrderItems,
                x => x.DeliveryMethod!);
            AddOrderByDescending(x => x.OrderDate);
        }

        public OrederSpecification(string email, int orderId)
            : base(o => o.BuyerEmail == email && o.Id == orderId)
        {
            AddInclude($"{nameof(Order.OrderItems)}", $"{nameof(Order.DeliveryMethod)}");  // you can use it with thenInclude by the write the prop.nestedProp
        }
    }
}
