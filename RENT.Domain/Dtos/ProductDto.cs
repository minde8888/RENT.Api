namespace RENT.Domain.Dtos.RequestDto
{
    public class ProductDto
    {
        public Guid ProductsId { get; set; }
        public string ImageHeight { get; set; }
        public string ImageWidth { get; set; }
        public string ImageName { get; set; }
        public string Price { get; set; }
        public string Size { get; set; }
        public string Place { get; set; }
        public string ProductCode { get; set; }
        public List<string> ImageSrc { get; set; }
        public Guid SellerId { get; set; }
        public ICollection<CategoriesDto> CategoriesDto { get; set; }
        public PostsDto PostsDto { get; set; }
    }
}
