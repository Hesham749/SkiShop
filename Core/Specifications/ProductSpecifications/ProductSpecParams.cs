using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.ProductSpecifications
{
    public class ProductSpecParams
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

        private int _pageSize = 6;
        private int _pageIndex = 1;
        private const int _MaxPageSize = 50;
        public int PageIndex { get => _pageIndex; set => _pageIndex = Math.Max(value, 1); }
        public int PageSize { get => _pageSize; set => _pageSize = Math.Min(Math.Max(value, 0), _MaxPageSize); }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set { _search = value ?? ""; }
        }

    }
}
