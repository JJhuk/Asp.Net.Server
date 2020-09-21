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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=userdb;Username=postgres;Password=password")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}