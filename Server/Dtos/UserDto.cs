using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace Server.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        
        public bool IsDeleted { get; set; } = false;
    }
}