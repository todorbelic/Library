using BookStoreClassLibrary.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStoreClassLibrary.Core.Entities
{
    public class User : IdentityUser, IAuditable
    {

        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public byte[]? Avatar { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }

        public User()
        {
        }
    }
}
