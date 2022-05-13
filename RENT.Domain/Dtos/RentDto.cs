using Microsoft.AspNetCore.Http;
using System;
using WTP.Domain.Entities;

namespace WTP.Domain.Dtos
{
    public class RentDto
    {
        public Guid RentId { get; set; }
        public string ImageName { get; set; }
        public string ImageSrc { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Title { get; set; }
        public Project Project { get; set; }
    }
}
