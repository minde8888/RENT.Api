using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IPostRepository
    {
        public Task AddItemAsync(Post post);
        public Task DeleteItem(Guid Id);
        public Task<List<Post>> GetItemIdAsync(Guid Id);
        public Task<List<Post>> GetItemAsync(string ImageSrc);
        public Task UpdateItem(Guid Id, Post post);
        public Task<IEnumerable<Post>> Search(string name);
        public void DeleteImage(string imagePath);
    }
}
