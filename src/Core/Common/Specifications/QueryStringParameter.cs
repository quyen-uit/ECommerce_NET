namespace Core.Common.Specifications
{
    public abstract class QueryStringParameter
    {
        private const int MaxPageSize = 50;
        public string? Sort { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = _pageSize > MaxPageSize ? MaxPageSize : value;
        }
    }
}
