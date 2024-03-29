﻿namespace RENT.Domain.Dtos.RequestDto
{
    public class ProductsDto
    {
        public Guid ProductsId { get; set; }
        public string ImageName { get; set; }
        public string Price { get; set; }
        public string Size { get; set; }
        public string Place { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<string> ImageSrc { get; set; }
        public Guid SellerId { get; set; }
        public ICollection<CategoriesDto> CategoriesDto { get; set; }
        public PostDto PostDto { get; set; }

    }
}
