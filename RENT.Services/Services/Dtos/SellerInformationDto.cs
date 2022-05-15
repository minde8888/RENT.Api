using RENT.Domain.Dtos;

namespace RENT.Services.Services.Dtos
{
    public class UserInformationDto : BaseEntityDto
    {
        public Guid SellerId { get; set; }
        public Guid CustomersId { get; set; }
        public ICollection<SellerDto> Seller { get; set; }
        public ICollection<CustomersDto> Customers { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}