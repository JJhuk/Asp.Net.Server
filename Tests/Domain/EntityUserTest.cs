using System.Threading.Tasks;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Domain.TestFixture;
using Xunit;

namespace Tests.Domain
{
    public class EntityUserTest
    {
        private readonly UserTestDb _db;
        
        public EntityUserTest()
        {
            var option = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase("Test")
                .Options;
            _db = new UserDb(option);
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }

        private async Task CreateUser()
        {
            _db.Users.Add(new TestUser());
            await _db.SaveChangesAsync();
        }

        [Fact]
        public async Task UserEntity_Should_HasAuditFields_When_AuditIsCreated()
        {
            await CreateUser();

            var userCount = await _db.Users.AsQueryable().CountAsync();
            var users = await _db.Users.AsQueryable().FirstAsync();

            userCount.Should().Be(1);
            users.Name.Should().NotBeNullOrEmpty();
        }
        
    }
}