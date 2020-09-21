using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
    }

}