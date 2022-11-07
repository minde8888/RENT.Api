using RENT.Domain.Entities;

namespace RENT.Data.Interfaces.IServices
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
