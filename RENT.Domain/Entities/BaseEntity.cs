using System.ComponentModel.DataAnnotations.Schema;
using RENT.Domain.Entities.Auth;

namespace RENT.Domain.Entities
{
    public class BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Occupation { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual Address Address { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
    }
}