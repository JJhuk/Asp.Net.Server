using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public interface IEntity
    {
        bool IsDeleted { get; set; }
        DateTime? CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }

    public class User : IEntity
    {
        public int Id { get; set; }

        [Required] public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Eamil Adress")]
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDateTime { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}