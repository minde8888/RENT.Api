using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Data.Interfaces
{
    public interface ICompressimage
    {
        public void Resize(string imagePath, string imageName, IFormFile imageFile);
    }
}
