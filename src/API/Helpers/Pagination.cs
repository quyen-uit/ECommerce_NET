namespace API.Helpers
{
    public class Pagination<T> where T : class 
    {
        public Pagination(int pageIndex, int pageSize, int pageCount, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            PageCount = pageCount;
            TotalPages = (int)Math.Ceiling(pageCount / (double)pageSize);
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int TotalPages { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
