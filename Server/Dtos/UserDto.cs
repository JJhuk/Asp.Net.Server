using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace Server.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        
        [Required]
        [Remote(action: "VerifyUserName",controller:"UsersController")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Adress")]
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public bool IsDeleted { get; set; } = false;
    }
}