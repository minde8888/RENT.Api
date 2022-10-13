using RENT.Data.Filter;

namespace RENT.Data.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
