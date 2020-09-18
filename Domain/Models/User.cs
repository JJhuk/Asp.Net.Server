using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Eamil Adress")]
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
} 
