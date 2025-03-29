using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.ProductSpecifications
{
    public class ProductSpecParams : PagingParams
    {
        private ICollection<string> _brands = [];

        public ICollection<string> Brands
        {
            get => _brands;
            set => _brands = [.. value.SelectMany(b=>b.Split(',',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))];
        }

        private ICollection<string> _Types = [];

        public ICollection<string> Types
        {
            get => _Types;
            set => _Types = [.. value.SelectMany(b=>b.Split(',',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))];
        }

        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set { _search = value ?? ""; }
        }

    }
}
