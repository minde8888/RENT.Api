using RENT.Domain.Dtos.RequestDto;

namespace RENT.Domain.Dtos.ResponseDto
{
    public class ProductResponseDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int FirstPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int? NextPage { get; set; }
        public int? PreviousPage { get; set; }
        public List<ProductsDto> ProductDto { get; set; }

    }
}
