
namespace Core.Specifications
{
    public class PagingParams
    {
        private int _pageSize = 6;
        private int _pageIndex = 1;
        private const int _MaxPageSize = 50;
        public int PageIndex { get => _pageIndex; set => _pageIndex = Math.Max(value, 1); }
        public int PageSize { get => _pageSize; set => _pageSize = Math.Min(Math.Max(value, 0), _MaxPageSize); }

    }
}
