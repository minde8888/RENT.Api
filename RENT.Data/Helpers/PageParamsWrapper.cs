using RENT.Data.Interfaces.IServices;
using RENT.Domain.Entities;

namespace RENT.Data.Helpers
{
    public class PageParamsWrapper<T>
    {
        public PageParamsWrapper(List<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            PagedData = pagedData ?? throw new ArgumentNullException(nameof(pagedData));
            ValidFilter = validFilter ?? throw new ArgumentNullException(nameof(validFilter));
            UriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
            Route = route ?? throw new ArgumentNullException(nameof(route));
            TotalRecords = totalRecords;
        }

        public List<T> PagedData { get; private set; }
        public PaginationFilter ValidFilter { get; private set; }
        public int TotalRecords { get; private set; }
        public IUriService UriService { get; private set; }
        public string Route { get; private set; }
    }
}
