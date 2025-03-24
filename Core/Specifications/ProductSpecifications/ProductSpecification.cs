using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications.ProductSpecifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams specParams) : base(p =>
             (p.Name.Contains(specParams.Search))
             &&
             (specParams.Brands.Count == 0 || specParams.Brands.Contains(p.Brand))
             &&
            (specParams.Types.Count == 0 || specParams.Types.Contains(p.Type))
            )
        {
            AddPagination(((specParams.PageIndex - 1) * specParams.PageSize), specParams.PageSize);

            switch (specParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }

        }
    }
}
