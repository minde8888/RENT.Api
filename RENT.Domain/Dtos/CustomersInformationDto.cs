

namespace RENT.Domain.Dtos
{
    public class CustomersInformationDto : BaseEntityDto
    {
        public Guid CustomersId { get; set; }
        public string Token { get; set; }
    }
}
