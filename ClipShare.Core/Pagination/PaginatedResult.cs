using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipShare.Core.Pagination
{
    public class PaginatedResult<T>(IReadOnlyList<T> items, int totalItemsCount, int pageNumber, int pageSize, int totalPages) where T : class
    {
        public IReadOnlyList<T> Items { get; set; } = items;
        public int TotalItemsCount { get; set; } = totalItemsCount;
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
        public int TotalPages { get; set; } = totalPages;
    }
}
