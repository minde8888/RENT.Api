

namespace RENT.Domain.Entities
{
    public class Posts
    {
        public Guid PostsId { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public Guid? ProductsId { get; set; }
        public virtual Products Products { get; set; }

    }
}
