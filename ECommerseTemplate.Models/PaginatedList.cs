namespace ECommerseTemplate.Models
{
    public class PaginatedList<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItemCount { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedList(IEnumerable<T> items, int pageNumber, int totalPages, int totalItemCount)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = totalPages;
            TotalItemCount = totalItemCount;
        }
    }
}
