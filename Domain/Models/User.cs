using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required]
        [Remote(action: "VerifyUserName",controller:"UsersController")]
        public string Username { get; set; }
        
        
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Eamil Adress")]
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
} 
