using RENT.Domain.Entities;
using RENT.Domain.Entities.Wrappers;

namespace RENT.Data.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedResponse<T>(PageParamsWrapper<T> inClassName)
        {
            var response = new PagedResponse<List<T>>(inClassName.PagedData, inClassName.ValidFilter.PageNumber, inClassName.ValidFilter.PageSize);
            var totalPages = ((double)inClassName.TotalRecords / (double)inClassName.ValidFilter.PageSize);
            var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            response.NextPage =
                inClassName.ValidFilter.PageNumber >= 1 && inClassName.ValidFilter.PageNumber < roundedTotalPages
                ? inClassName.UriService.GetPageUri(new PaginationFilter(inClassName.ValidFilter.PageNumber + 1, inClassName.ValidFilter.PageSize), inClassName.Route)
                : null;
            response.PreviousPage =
                inClassName.ValidFilter.PageNumber - 1 >= 1 && inClassName.ValidFilter.PageNumber <= roundedTotalPages
                ? inClassName.UriService.GetPageUri(new PaginationFilter(inClassName.ValidFilter.PageNumber - 1, inClassName.ValidFilter.PageSize), inClassName.Route)
                : null;
            response.FirstPage = inClassName.UriService.GetPageUri(new PaginationFilter(1, inClassName.ValidFilter.PageSize), inClassName.Route);
            response.LastPage = inClassName.UriService.GetPageUri(new PaginationFilter(roundedTotalPages, inClassName.ValidFilter.PageSize), inClassName.Route);
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = inClassName.TotalRecords;

            response.NexPage = inClassName.ValidFilter.PageNumber >= 1 && inClassName.ValidFilter.PageNumber < roundedTotalPages
                ? new PaginationFilter(inClassName.ValidFilter.PageNumber + 1, inClassName.ValidFilter.PageSize).PageNumber : null;

            response.PrevPage = inClassName.ValidFilter.PageNumber - 1 >= 1 && inClassName.ValidFilter.PageNumber <= roundedTotalPages
                ? new PaginationFilter(inClassName.ValidFilter.PageNumber - 1, inClassName.ValidFilter.PageSize).PageNumber : null;
            return response;
        }
    }
}