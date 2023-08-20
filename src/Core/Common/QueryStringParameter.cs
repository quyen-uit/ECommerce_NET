namespace Core.Common
{
    public abstract class QueryStringParameter
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = _pageSize > MaxPageSize ? MaxPageSize : value;
        }
    }
}
