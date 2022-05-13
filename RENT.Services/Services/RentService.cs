using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WTP.Domain.Dtos;


namespace WTP.Services.Services
{
    public class RentService
    {
        public void GetImagesAsync(List<RentDto> rentDto, string ImageSrc)
        {

            foreach (var toolImage in rentDto)
            {
                string toolImageName = toolImage.ImageName;
                toolImage.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, toolImageName);
            }
        }
    }
}
