using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class ShippingAddress
    {
        [MaxLength(250)]
        public required string Name { get; set; }

        [MaxLength(250)]
        public required string Line1 { get; set; }

        [MaxLength(250)]
        public string? Line2 { get; set; }

        [MaxLength(250)]
        public required string City { get; set; }

        [MaxLength(250)]
        public required string State { get; set; }

        [MaxLength(250)]
        public required string PostalCode { get; set; }

        [MaxLength(250)]
        public required string Country { get; set; }
    }
}
