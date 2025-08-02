namespace ClipShare.Core.Pagination
{
    // This is the class that we send back to the browser
    public class PaginatedResult<T>(IReadOnlyList<T> items, int totalItemsCount, int pageNumber, int pageSize, int totalPages) where T : class
    {
        public IReadOnlyList<T> Items { get; set; } = items;
        public int TotalItemsCount { get; set; } = totalItemsCount;
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
        public int TotalPages { get; set; } = totalPages;
    }
}
