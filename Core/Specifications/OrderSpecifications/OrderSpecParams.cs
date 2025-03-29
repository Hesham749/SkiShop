using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.OrderSpecifications
{
    public class OrderSpecParams : PagingParams
    {
        public string? Filter { get; set; }
    }
}
